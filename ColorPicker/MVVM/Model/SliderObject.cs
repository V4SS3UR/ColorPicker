using ColorPicker.Core;
using System.Windows.Media;

namespace ColorPicker.MVVM.Model
{
    public class SliderObject : ObservableObject
    {
        public delegate void ValueChangedHandler(object sender);
        public event ValueChangedHandler OnValueChanged;
        private void OnValueChangedCallBack()
        {
            if (OnValueChanged == null) return;
            OnValueChanged(this);
        }

        private SolidColorBrush _solidColorBrush; public SolidColorBrush SolidColorBrush
        {
            get { return _solidColorBrush; }
            set { _solidColorBrush = value; OnPropertyChanged(); }
        }
        private double _value; public double Value
        {
            get { return _value; }
            set
            {
                value = value > 1 ? 0.999 : value;
                value = value > 0.001 ? value : 0.001;
                _value = value;

                OnValueChangedCallBack();

                OnPropertyChanged();
            }
        }


        public bool Freeze { get; set; }


        public SliderObject(ValueChangedHandler valueChangedHandler = null)
        {
            this.OnValueChanged += valueChangedHandler;
        }

        public void SetBrush(SolidColorBrush solidColorBrush)
        {
            if(!Freeze)
                this.SolidColorBrush = solidColorBrush;
        }

        public void SetValue(double value)
        {
            if (!Freeze)
                this.Value = value;
        }
    }
}