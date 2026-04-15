using System;

namespace Environment.Base
{
    public class BasePresenter : IDisposable
    {
        private readonly BaseModel _model;
        private readonly BaseView _view;

        public BasePresenter(BaseModel model, BaseView view)
        {
            _model = model;
            _view = view;

            _model.OnCapturingProgressChanged += HandleCapturingProgressChanged;
        }

        private void HandleCapturingProgressChanged()
        {
            _view.UpdateCapturingProgressSlider(_model.CapturingRate, _model.IsRecapturing, _model.IsPlayerInBase);
            _view.UpdateCapturingProgressText(_model.CapturingRate);
        }

        public void Dispose()
        {
            _model.OnCapturingProgressChanged -= HandleCapturingProgressChanged;
        }
    }
}