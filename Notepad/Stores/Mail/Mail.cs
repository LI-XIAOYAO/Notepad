using MailKit;
using MailKit.Net.Imap;
using MailKit.Security;
using MimeKit;
using Newtonsoft.Json;
using Notepad.Entities;
using Notepad.Properties;
using System.Net.Sockets;
using System.Text;

namespace Notepad.Stores
{
    /// <summary>
    /// Mail
    /// </summary>
    internal class Mail : IStore
    {
        private const string PATH = nameof(Notepad);
        private static readonly object _lock = new();
        private InternetAddress? _address;
        private EmailUser _user;

        /// <summary>
        /// Cache
        /// </summary>
        public Notes Cache { get; private set; } = new([]);

        /// <summary>
        /// EmailServer
        /// </summary>
        public IUser User => _user;

        /// <summary>
        /// ImapClient
        /// </summary>
        private ImapClient ImapClient { get; set; } = new();

        /// <summary>
        /// LoginAsync
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task LoginAsync(IUser user, CancellationToken cancellationToken = default)
        {
            _user = (EmailUser)user;
            _address = InternetAddress.Parse(user.Account);

            await ConnectAsync(1, cancellationToken);
        }

        /// <summary>
        /// ConnectAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            if (!ImapClient.IsConnected)
            {
                try
                {
                    await ImapClient.ConnectAsync(_user.Host, _user.Port, true, cancellationToken);
                }
                catch (Exception ex)
                {
                    throw new Exception(Resources.Mall_ConnectFailed, ex);
                }
            }

            if (!ImapClient.IsAuthenticated)
            {
                try
                {
                    await ImapClient.AuthenticateAsync(User.Account, User.Pwd, cancellationToken);
                    await ImapClient.IdentifyAsync(new ImapImplementation
                    {
                        Vendor = nameof(Notepad)
                    }, cancellationToken);
                }
                catch (AuthenticationException)
                {
                    throw new Exception(Resources.Mall_AuthFailed_IncorrectPassword);
                }
                catch (Exception ex)
                {
                    throw new Exception(Resources.Mall_AuthFailed, ex);
                }
            }
        }

        /// <summary>
        /// ConnectAsync
        /// </summary>
        /// <param name="retry"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<IMailFolder> ConnectAsync(int retry, CancellationToken cancellationToken = default)
        {
            Monitor.Enter(_lock);

            try
            {
                await ConnectAsync(cancellationToken);

                return await GetOrAddFolderAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                if (retry-- > 0)
                {
                    if (0 == retry || (retry > 0 && (ex is ImapProtocolException || ex.InnerException is ImapProtocolException || ex is SocketException || ex.InnerException is SocketException)))
                    {
                        await ImapClient.DisconnectAsync(true, cancellationToken);
                        ImapClient.Dispose();

                        ImapClient = new ImapClient();

                        return await ConnectAsync(retry, cancellationToken);
                    }

                    return await ConnectAsync(retry, cancellationToken);
                }

                throw;
            }
            finally
            {
                Monitor.Exit(_lock);
            }
        }

        /// <summary>
        /// GetOrAddFolderAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<IMailFolder> GetOrAddFolderAsync(CancellationToken cancellationToken = default)
        {
            return (await ImapClient.GetFoldersAsync(new FolderNamespace('/', string.Empty), cancellationToken: cancellationToken)).FirstOrDefault(c => c.Name == PATH)
                ?? await (await ImapClient.GetFolderAsync(string.Empty, cancellationToken)).CreateAsync(PATH, true, cancellationToken);
        }

        /// <summary>
        /// GetNotesAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Notes> GetNotesAsync(CancellationToken cancellationToken = default)
        {
            IMailFolder? folder = null;

            try
            {
                folder = await ConnectAsync(2, cancellationToken);
                await folder.OpenAsync(FolderAccess.ReadWrite, cancellationToken);

                var message = folder.OrderByDescending(c => c.Date).FirstOrDefault(c => c.Subject == PATH);
                if (null != message)
                {
                    var mimePart = message.Attachments.FirstOrDefault(c => c is MimePart mimePart && PATH == mimePart.FileName) as MimePart;
                    if (null != mimePart)
                    {
                        using var streamReader = new StreamReader(mimePart.Content.Stream);
                        var body = await streamReader.ReadToEndAsync(cancellationToken);

                        if (null != body)
                        {
                            body = Encoding.UTF8.GetString(Convert.FromBase64String(body));

                            if (null != body)
                            {
                                Cache = JsonConvert.DeserializeObject<Notes>(body)!;
                            }
                        }
                    }
                }
            }
            catch (ImapCommandException ex)
            {
                throw new Exception(ex.ResponseText);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (null != folder)
                {
                    await folder.CloseAsync(expunge: false, cancellationToken);
                }
            }

            return Cache;
        }

        /// <summary>
        /// SaveNotesAsync
        /// </summary>
        /// <param name="notes"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task SaveNotesAsync(Notes notes, CancellationToken cancellationToken = default)
        {
            Monitor.Enter(_lock);
            IMailFolder? folder = null;

            try
            {
                folder = await ConnectAsync(2, cancellationToken);
                await folder.OpenAsync(FolderAccess.ReadWrite, cancellationToken);

                var bodyBuilder = new BodyBuilder();
                await bodyBuilder.Attachments.AddAsync(PATH, new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(notes))), cancellationToken);

                await folder.AppendAsync(new AppendRequest(new MimeMessage
                {
                    From = { _address },
                    To = { _address },
                    Subject = PATH,
                    Body = bodyBuilder.ToMessageBody()
                }), cancellationToken);
                await folder.SetFlagsAsync([.. Enumerable.Range(0, folder.Count)], MessageFlags.Deleted, false, cancellationToken);
                await folder.ExpungeAsync(cancellationToken);

                folder = await GetOrAddFolderAsync(cancellationToken);

                await folder.SetFlagsAsync([.. Enumerable.Range(0, folder.Count)], MessageFlags.Seen, false, cancellationToken);
                await folder.ExpungeAsync(cancellationToken);
            }
            finally
            {
                if (null != folder)
                {
                    await folder.CloseAsync(false, cancellationToken);
                }

                Monitor.Exit(_lock);
            }
        }
    }
}