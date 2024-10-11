using AntdUI;
using Notepad.Entities;
using Notepad.Enums;
using Notepad.Extensions;
using Notepad.Properties;
using System.Diagnostics;

namespace Notepad.Controls
{
    /// <summary>
    /// AddNotesWindow
    /// </summary>
    internal class AddNotesWindow : DialogWindow
    {
        private Note? _addNotes;
        private readonly Input _titleInput;
        private readonly StackPanel _notesPanel;
        private readonly AntdUI.Button _operationButton;
        private AntdUI.Button? _saveButton;
        private bool _isEdit;

        /// <summary>
        /// EditChange
        /// </summary>
        private event EventHandler? EditChange;

        /// <summary>
        /// Added
        /// </summary>
        public event EventHandler? Added;

        /// <summary>
        /// AddNotesWindow
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="dividerOptions"></param>
        public AddNotesWindow(Control parent, string title, int width = 500, int height = 250, DialogWindowDividerOptions dividerOptions = DialogWindowDividerOptions.TopBottom)
            : base(parent, width, height, title, dividerOptions: dividerOptions)
        {
            var cache = Store.Cache;
            var titlePanel = new AntdUI.Panel
            {
                Dock = DockStyle.Top,
                Height = 30,
                TabStop = false
            };
            titlePanel.Controls.Add(new AntdUI.Label
            {
                Dock = DockStyle.Left,
                Text = $"{Resources.Text_Name}:",
                Width = 40,
                Height = titlePanel.Height,
                BackColor = Color.Transparent,
                TabStop = false
            });

            _titleInput = new Input
            {
                Dock = DockStyle.Fill,
                Height = titlePanel.Height,
                Padding = new Padding(40, 0, 0, 0),
                WaveSize = 0,
                BorderWidth = 1F,
                Radius = 3,
                CaretSpeed = 700,
                BorderHover = Style.Db.BorderColor,
                BorderActive = Style.Db.BorderColor
            };

            _titleInput.GotFocus += OnTitleInputGotFocus;
            titlePanel.Controls.Add(_titleInput);

            _notesPanel = new StackPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Vertical = true,
                TabStop = false
            };

            ContentPanel!.Controls.Add(_notesPanel);
            ContentPanel.Controls.Add(new AntdUI.Panel
            {
                Dock = DockStyle.Top,
                Height = 15,
                Radius = 0,
                TabStop = false
            });

            _operationButton = new AntdUI.Button
            {
                Dock = DockStyle.Bottom,
                Height = 32,
                IconSize = new Size(30, 30),
                IconSvg = Resources.Svg_Add,
                Ghost = true,
                WaveSize = 0,
                ForeColor = Style.Db.Primary,
                Radius = 3,
                Tag = false,
                TabStop = false
            };
            _operationButton.MouseClick += (_, _) =>
            {
                if ((bool)_operationButton.Tag)
                {
                    ChangeState(isEditLoad: false);

                    return;
                }

                if (null == _addNotes)
                {
                    _addNotes = new(_titleInput.Text, notes: [])
                    {
                        Id = cache.GetNextId()
                    };
                }

                LoadAddWindow();
            };

            ContentPanel.Controls.Add(_operationButton);
            ContentPanel.Controls.Add(titlePanel);

            AddButton(Resources.Button_Save, TTypeMini.Primary, (d, b) =>
            {
                _saveButton = b;

                b.MouseClick += async (_, _) =>
                {
                    if ((bool)_operationButton.Tag)
                    {
                        d.Close();

                        return;
                    }

                    if (_titleInput.Text.IsNullOrBlank())
                    {
                        _titleInput.BorderColor = Color.Red;
                    }
                    else
                    {
                        var width = b.Width;
                        b.Loading = true;
                        b.Width += 15;

                        if (null == _addNotes)
                        {
                            _addNotes = new(_titleInput.Text, notes: [])
                            {
                                Id = cache.GetNextId()
                            };
                        }
                        else
                        {
                            _addNotes.Title = _titleInput.Text;
                        }

                        var temp = cache.Clone();

                        _addNotes.LastTime = DateTimeOffset.Now;

                        if (_isEdit)
                        {
                            temp.Update(_addNotes);
                        }
                        else
                        {
                            temp.Add(_addNotes);
                        }

                        Exception? exception = null;

                        try
                        {
                            var data = temp.Datas;

                            await Store.SaveNotesAsync(temp.Encrypt());

                            cache.Update(data);

                            d.Close();

                            Added?.Invoke(null, EventArgs.Empty);
                        }
                        catch (Exception ex)
                        {
                            exception = ex;

                            b.Loading = false;
                            b.Width = width;
                        }

                        parent.Loading(null == exception ? Resources.Loading_SaveSuccess : $"{Resources.Loading_SaveFailed}{exception.Message}", 0, false, iconType: null == exception ? TType.Success : TType.Error).CloseAfter(1000);
                    }
                };
            });

            titlePanel.Radius = Radius;

            AddAdjustSizeButton();
        }

        /// <summary>
        /// LoadNotes
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public AddNotesWindow LoadNotes(Note note, bool isEditLoad = true)
        {
            if (isEditLoad)
            {
                _isEdit = true;
                ChangeState(note);
            }

            if (note.HasNotes)
            {
                var i = _notesPanel.Controls.Count > 0 ? -1 : 0;

                foreach (var item in note.Notes!)
                {
                    var notePanel = new AntdUI.Panel
                    {
                        Height = 25,
                        Margin = new Padding(),
                        Radius = 3,
                        TabStop = false
                    };
                    var deleteButton = new AntdUI.Button
                    {
                        Name = "Delete",
                        Dock = DockStyle.Right,
                        Size = new Size(notePanel.Height, notePanel.Height),
                        IconSize = new Size(20, 20),
                        IconSvg = Resources.Svg_Delete,
                        Ghost = true,
                        WaveSize = 0,
                        ForeColor = Style.Db.Error,
                        Radius = 3,
                        BorderWidth = 0,
                        TabStop = false,
                        Visible = !isEditLoad,
                        Tag = item
                    };
                    var editButton = new AntdUI.Button
                    {
                        Dock = DockStyle.Right,
                        Size = new Size(notePanel.Height, notePanel.Height),
                        IconSize = new Size(20, 20),
                        IconSvg = Resources.Svg_Edit,
                        Ghost = true,
                        WaveSize = 0,
                        ForeColor = Style.Db.Primary,
                        Radius = 3,
                        BorderWidth = 0,
                        TabStop = false,
                        Visible = !isEditLoad,
                        Tag = item
                    };
                    var viewButton = new AntdUI.Button
                    {
                        Dock = DockStyle.Right,
                        Size = new Size(notePanel.Height, notePanel.Height),
                        IconSize = new Size(12, 12),
                        IconSvg = "RightOutlined",
                        Ghost = true,
                        WaveSize = 0,
                        ForeColor = Style.Db.Primary,
                        Radius = 3,
                        BorderWidth = 0,
                        TabStop = false,
                        Visible = isEditLoad,
                        Tag = item
                    };

                    if (isEditLoad)
                    {
                        EditChange += (_, _) =>
                        {
                            viewButton.Visible = false;
                            editButton.Visible = true;
                            deleteButton.Visible = true;
                        };
                    }

                    deleteButton.MouseClick += OnDelete;
                    editButton.MouseClick += OnEdit;
                    viewButton.MouseClick += (_, _) => LoadAddWindow(note: item, isView: true);

                    var contentLabel = new AntdUI.Label
                    {
                        Name = nameof(item.Value),
                        Dock = DockStyle.Fill,
                        TextMultiLine = false,
                        AutoEllipsis = true,
                        ShowTooltip = false,
                        BackColor = Color.Transparent,
                        TabStop = false
                    };

                    notePanel.Controls.Add(contentLabel);

                    contentLabel.TextChanged += OnLinkDiscover;
                    contentLabel.Text = item.Value?.ReplaceNewLine(" ");

                    notePanel.Controls.Add(viewButton);
                    notePanel.Controls.Add(editButton);
                    notePanel.Controls.Add(deleteButton);

                    var margin = (int)(notePanel.Height * 0.2);
                    notePanel.Controls.Add(new Divider
                    {
                        Dock = DockStyle.Left,
                        Vertical = true,
                        Thickness = 1F,
                        Width = 10,
                        Margin = new Padding(0, margin, 0, margin),
                        BackColor = Color.Transparent,
                        TabStop = false
                    });

                    var titleLabel = new AntdUI.Label
                    {
                        Name = nameof(item.Title),
                        Dock = DockStyle.Left,
                        Text = item.Title,
                        Width = 50,
                        Padding = new Padding(5, 0, 0, 0),
                        AutoEllipsis = true,
                        TextMultiLine = false,
                        ShowTooltip = false,
                        BackColor = Color.Transparent,
                        TabStop = false
                    };

                    notePanel.Controls.Add(titleLabel);

                    Divider? divider = null;
                    if (0 != i && i < note.Notes.Count)
                    {
                        _notesPanel.Controls.Add(divider = new Divider
                        {
                            Thickness = 1F,
                            Height = 1,
                            TabStop = false
                        });
                    }

                    void OnPanelMouseEnter(object? s, EventArgs e)
                    {
                        notePanel.Back = Style.Db.PrimaryBg;
                    };
                    void OnPanelMouseLeave(object? s, EventArgs e)
                    {
                        notePanel.Back = null;
                    };

                    titleLabel.MouseEnter += OnPanelMouseEnter;
                    contentLabel.MouseEnter += OnPanelMouseEnter;
                    titleLabel.MouseLeave += OnPanelMouseLeave;
                    contentLabel.MouseLeave += OnPanelMouseLeave;
                    titleLabel.MouseDoubleClick += Label_MouseDoubleClick;
                    contentLabel.MouseDoubleClick += Label_MouseDoubleClick;

                    titleLabel.HoverLinePopover();
                    contentLabel.HoverLinePopover(true);

                    _notesPanel.Controls.Add(notePanel);
                    divider?.BringToFront();
                    notePanel.BringToFront();

                    i++;
                }
            }

            return this;
        }

        /// <summary>
        /// LoadAddWindow
        /// </summary>
        /// <param name="button"></param>
        /// <param name="note"></param>
        /// <param name="isView"></param>
        private void LoadAddWindow(AntdUI.Button? button = null, Note? note = null, bool isView = false)
        {
            var panel = new AntdUI.Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 10, 10, 0),
                TabStop = false,
            };
            var content = new Input
            {
                Dock = DockStyle.Fill,
                Text = note?.Value ?? string.Empty,
                PlaceholderText = Resources.Text_Nothing,
                Multiline = true,
                AutoScroll = true,
                WaveSize = 0,
                BorderWidth = 1F,
                Radius = 3,
                BorderHover = Style.Db.BorderColor,
                BorderActive = Style.Db.BorderColor,
                CaretSpeed = 700,
                CaretColor = isView ? Color.Transparent : null,
                TabIndex = 1,
                ReadOnly = isView
            };
            var title = new Input
            {
                Text = note?.Title ?? string.Empty,
                PlaceholderText = Resources.Text_Name,
                Height = 30,
                Dock = DockStyle.Top,
                WaveSize = 0,
                BorderWidth = 1F,
                Radius = 3,
                BorderHover = Style.Db.BorderColor,
                BorderActive = Style.Db.BorderColor,
                CaretSpeed = 700,
                CaretColor = isView ? Color.Transparent : null,
                TabIndex = 0,
                ReadOnly = isView
            };
            title.GotFocus += OnTitleInputGotFocus;

            panel.Controls.Add(content);
            panel.Controls.Add(new AntdUI.Panel
            {
                Height = 10,
                Dock = DockStyle.Top,
                Radius = 0,
                TabStop = false
            });
            panel.Controls.Add(title);

            new DialogWindow(ParentControl, 500, 250, $"{(isView ? Resources.Text_View : (null == note ? Resources.Text_New : Resources.Text_Edit))}", panel, DialogWindowDividerOptions.Top)
                .AddButton(isView ? Resources.Button_Close : Resources.Button_Save, isView ? TTypeMini.Default : TTypeMini.Primary, (d, b) =>
                {
                    b.MouseClick += (_, _) =>
                    {
                        if (isView || title.Text.IsNullOrBlank() && content.Text.IsNullOrBlank())
                        {
                            d.Close();
                        }
                        else if (title.Text.IsNullOrBlank())
                        {
                            title.BorderColor = Color.Red;
                        }
                        else
                        {
                            d.Close();

                            if (null == note)
                            {
                                note = new Note(title.Text, content.Text)
                                {
                                    Id = _addNotes!.GetNextId()
                                };

                                _addNotes!.Notes!.Add(note);

                                LoadNotes(new(string.Empty, notes: [note]), false);
                            }
                            else
                            {
                                note.Title = title.Text;
                                note.Value = content.Text;

                                var controls = button!.Parent!.Controls;
                                ((AntdUI.Label)controls[nameof(Note.Title)]!).Text = title.Text;
                                ((AntdUI.Label)controls[nameof(Note.Value)]!).Text = content.Text.ReplaceNewLine(" ");
                            }
                        }
                    };
                })
                .AddAdjustSizeButton(IsAdjustSize)
                .Show(true);
        }

        /// <summary>
        /// ChangeState
        /// </summary>
        /// <param name="note"></param>
        /// <param name="isEditLoad"></param>
        private void ChangeState(Note? note = null, bool isEditLoad = true)
        {
            if (isEditLoad)
            {
                _addNotes = note;
                _titleInput.Text = note!.Title;
                _titleInput.BorderWidth = 0;
                _operationButton.IconSvg = Resources.Svg_Edit;
                _saveButton!.Text = Resources.Button_Close;
                _saveButton.Type = TTypeMini.Default;
                _saveButton.BorderWidth = 1.5F;
            }
            else
            {
                _titleInput.BorderWidth = 1;
                _operationButton.IconSvg = Resources.Svg_Add;
                _saveButton!.Text = Resources.Button_Save;
                _saveButton.Type = TTypeMini.Primary;
                _saveButton.BorderWidth = 0F;

                if (HeaderPanel.Controls["Title"] is AntdUI.Label titleLabel)
                {
                    titleLabel.Text = Resources.Text_Edit;
                }

                EditChange?.Invoke(this, EventArgs.Empty);
            }

            _titleInput.Enabled = !isEditLoad;
            _operationButton.Tag = isEditLoad;
        }

        /// <summary>
        /// OnLinkDiscover
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void OnLinkDiscover(object? s, EventArgs e)
        {
            var label = (AntdUI.Label)s!;

            var notePanel = label.Parent!;
            var linkButton = notePanel.Controls["LinkButton"];

            if (!label.Text.IsNullOrBlank())
            {
                var urls = label.Text!.MatchUrls();
                if (urls.Count > 0)
                {
                    if (null == linkButton)
                    {
                        linkButton = new AntdUI.Button
                        {
                            Name = "LinkButton",
                            Dock = DockStyle.Right,
                            Size = new Size(notePanel.Height, notePanel.Height),
                            IconSize = new Size(16, 16),
                            IconSvg = "LinkOutlined",
                            Ghost = true,
                            WaveSize = 0,
                            ForeColor = Style.Db.Primary,
                            Radius = 3,
                            BorderWidth = 0,
                            TabStop = false
                        };
                        linkButton.MouseClick += (_, _) =>
                        {
                            new AntdUI.ContextMenuStrip.Config(label, c => Process.Start(new ProcessStartInfo
                            {
                                FileName = c.Tag!.ToString()!,
                                UseShellExecute = true
                            }), [])
                            {
                                Align = TAlign.Top,
                                Radius = 3
                            }.Apply(c =>
                            {
                                c.Items = urls.Select(c =>
                                {
                                    var text = c;
                                    var size = TextRenderer.MeasureText(c, label.Font);
                                    if (size.Width > Width)
                                    {
                                        size = TextRenderer.MeasureText(" ", label.Font);
                                        var count = Width / size.Width;

                                        if (count < c.Length)
                                        {
                                            text = $"{text.AsSpan()[..count]}...";
                                        }
                                    }

                                    return new ContextMenuStripItem(text)
                                    {
                                        Tag = c
                                    };
                                }).ToArray();
                            }).open()
                            .Apply(c => c!.MouseLeave += (_, _) => c.Dispose());
                        };

                        notePanel.Controls.Add(linkButton);
                    }
                }
                else
                {
                    notePanel.Controls.Remove(linkButton);
                }
            }
            else
            {
                notePanel.Controls.Remove(linkButton);
            }
        }

        /// <summary>
        /// OnDelete
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void OnDelete(object? s, EventArgs e)
        {
            if (s is AntdUI.Button button)
            {
                _addNotes?.Notes?.Remove((Note)button.Tag!);

                var panelIndex = _notesPanel.Controls.IndexOf(button.Parent);

                _notesPanel.Controls.RemoveAt(panelIndex + (panelIndex == _notesPanel.Controls.Count - 1 ? -1 : 1));
                _notesPanel.Controls.Remove(button.Parent);
            }
        }

        /// <summary>
        /// OnEdit
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void OnEdit(object? s, EventArgs e)
        {
            if (s is AntdUI.Button button)
            {
                LoadAddWindow(button, (Note?)button.Tag);
            }
        }

        /// <summary>
        /// OnTitleInputGotFocus
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void OnTitleInputGotFocus(object? s, EventArgs e) => ((Input)s!).BorderColor = Style.Db.BorderColor;

        /// <summary>
        /// Label_MouseDoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            if (sender is AntdUI.Label label && !string.IsNullOrWhiteSpace(label.Text))
            {
                string? tips = null;

                try
                {
                    Clipboard.SetText(label.Text);
                }
                catch
                {
                    tips = Resources.CopyFailed;
                }

                label.Popover(tips ?? Resources.CopySuccess, 1, offset: new Rectangle(e.Location, new()));
            }
        }

        public override void HandleEvent(EventType id, object? tag)
        {
            foreach (Control control in _notesPanel.Controls)
            {
                if (control is AntdUI.Panel panel)
                {
                    foreach (Control item in panel.Controls)
                    {
                        if (item is AntdUI.Button button)
                        {
                            button.ForeColor = "Delete" == button.Name ? Style.Db.Error : Style.Db.Primary;
                        }
                    }
                }
            }

            _operationButton.ForeColor = Style.Db.Primary;

            base.HandleEvent(id, tag);
        }
    }
}