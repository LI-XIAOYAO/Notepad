namespace Notepad.Controls
{
    /// <summary>
    /// ControlsSelector
    /// </summary>
    internal class ControlsSelector : IDisposable
    {
        private readonly System.Windows.Forms.Timer _timer = new();
        private EventHandler? _timerEventHandler;
        private bool _disposed;

        /// <summary>
        /// IsSelected
        /// </summary>
        public bool IsSelected { get; private set; }

        /// <summary>
        /// SelectedCount
        /// </summary>
        public int SelectedCount { get; protected set; }

        /// <summary>
        /// Parent
        /// </summary>
        public Control? Parent { get; }

        /// <summary>
        /// Controls
        /// </summary>
        public Dictionary<Control, bool> Controls { get; } = [];

        /// <summary>
        /// Changed
        /// </summary>
        public event Action<Control, bool>? Changed;

        /// <summary>
        /// Click
        /// </summary>
        public event Action<Control>? Click;

        /// <summary>
        /// Reseted
        /// </summary>
        public event Action? Reseted;

        /// <summary>
        /// ControlSelector
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="longClickMillsInterval"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ControlsSelector(Control? parent = null, int longClickMillsInterval = 500)
        {
            Parent = parent;

            _timer.Interval = longClickMillsInterval < 500 ? 500 : longClickMillsInterval;
            _timer.Tick += (s, e) => _timerEventHandler?.Invoke(s, e);
        }

        /// <summary>
        /// AddControl
        /// </summary>
        /// <param name="control"></param>
        /// <param name="targetControl"></param>
        /// <param name="isAddChilds"></param>
        private void AddControl(Control control, Control targetControl, bool isAddChilds)
        {
            void OnClick(object? s, EventArgs e)
            {
                if (IsSelected)
                {
                    var isSelected = Controls[targetControl] = !Controls[targetControl];
                    if (isSelected)
                    {
                        SelectedCount++;
                    }
                    else
                    {
                        SelectedCount--;
                    }

                    if (0 == SelectedCount)
                    {
                        IsSelected = false;
                    }

                    Changed?.Invoke(targetControl, isSelected);
                }
                else
                {
                    Click?.Invoke(targetControl);
                }
            }

            control.MouseDown += (_, _) =>
            {
                if (null == Changed)
                {
                    return;
                }

                _timerEventHandler = (s, e) =>
                {
                    if (!IsSelected)
                    {
                        IsSelected = true;

                        if (targetControl.RectangleToScreen(targetControl.ClientRectangle).Contains(Control.MousePosition))
                        {
                            control.MouseClick -= OnClick;

                            OnClick(s, e);
                        }
                    }
                };

                _timer.Start();
            };
            control.MouseUp += (_, _) =>
            {
                _timer.Stop();

                if (null == Changed)
                {
                    return;
                }

                if (IsSelected)
                {
                    control.MouseClick -= OnClick;
                    control.MouseClick += OnClick;

                    if (0 == SelectedCount)
                    {
                        IsSelected = false;
                    }
                }
            };
            control.MouseClick += OnClick;
            control.Disposed += (_, _) =>
            {
                if (Controls.TryGetValue(control, out var isSelected))
                {
                    Controls.Remove(control);

                    if (isSelected)
                    {
                        SelectedCount--;
                    }
                }
            };

            if (isAddChilds)
            {
                foreach (Control item in control.Controls)
                {
                    AddControl(item, targetControl, isAddChilds);
                }
            }
        }

        /// <summary>
        /// AddControl
        /// </summary>
        /// <param name="control"></param>
        /// <param name="isAddChilds"></param>
        public void AddControl(Control control, bool isAddChilds = true)
        {
            Controls[control] = false;

            AddControl(control, control, isAddChilds);
        }

        /// <summary>
        /// ResetSelector
        /// </summary>
        public void ResetSelector()
        {
            Controls.Clear();

            SelectAll(false);

            Reseted?.Invoke();
        }

        /// <summary>
        /// SelectAll
        /// </summary>
        /// <param name="isSelected"></param>
        public void SelectAll(bool isSelected = true)
        {
            IsSelected = isSelected;
            SelectedCount = isSelected ? Controls.Count : 0;

            if (Controls.Count > 0)
            {
                Parent?.SuspendLayout();

                foreach (var control in Controls.Keys)
                {
                    if (Controls[control] != isSelected)
                    {
                        Controls[control] = isSelected;

                        if (!control.IsDisposed)
                        {
                            Changed?.Invoke(control, isSelected);
                        }
                    }
                }

                Parent?.ResumeLayout();
            }
        }

        /// <summary>
        /// SelectUn
        /// </summary>
        public void SelectUn()
        {
            SelectedCount = Controls.Count - SelectedCount;
            IsSelected = SelectedCount > 0;

            if (Controls.Count > 0)
            {
                Parent?.SuspendLayout();

                foreach (var control in Controls.Keys)
                {
                    Controls[control] = !Controls[control];

                    if (!control.IsDisposed)
                    {
                        Changed?.Invoke(control, Controls[control]);
                    }
                }

                Parent?.ResumeLayout();
            }
        }

        /// <summary>
        /// GetSelectedControls
        /// </summary>
        /// <returns></returns>
        public List<Control> GetSelectedControls()
        {
            return Controls.Where(c => c.Value).Select(c => c.Key).ToList();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _timer.Dispose();

                    ResetSelector();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}