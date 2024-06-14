using ColorPicker.Core;
using ColorPicker.MVVM.ViewModel;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;

namespace ColorPicker.MVVM.Model
{
    public class Project : ObservableObject
    {
       

        private string _name; public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        public SliderObject HueSliderObject { get; set; }
        public SliderObject SaturationSliderObject { get; set; }
        public SliderObject LightnessSliderObject { get; set; }

        private ColorObject _currentColor; public ColorObject CurrentColor
        {
            get { return _currentColor; }
            set
            {
                if (_currentColor != null)
                {
                    _currentColor.OnColorChanged -= On_ColorObjectColor_Changed;
                }

                _currentColor = value;

                _currentColor.OnColorChanged += On_ColorObjectColor_Changed;
                Update_HslSlidersValue_FromBrush(this.CurrentColor);
                Update_HslSlidersBrush_FromBrush(this.CurrentColor);

                OnPropertyChanged();
            }
        }


        
        public ObservableCollection<ColorObject> ColorHistory { get; set; }
        
        public ObservableCollection<ColorObject> ColorPinned { get; set; }

        public Project()
        {
            this.HueSliderObject = new SliderObject(OnHueValueChanged);
            this.SaturationSliderObject = new SliderObject(OnSaturationValueChanged);
            this.LightnessSliderObject = new SliderObject(OnLightnessValueChanged);

            this.CurrentColor = new ColorObject("New color");
            this.ColorHistory = new ObservableCollection<ColorObject>();
            this.ColorPinned = new ObservableCollection<ColorObject>();        
        }

        public void OnHueValueChanged(object sender)
        {
            HueSliderObject.Freeze = true;
            UpdateCurrentColorFromHslSlider(h: ((SliderObject)sender).Value);
            HueSliderObject.Freeze = false;
        }

        public void OnSaturationValueChanged(object sender)
        {
            SaturationSliderObject.Freeze = true;
            UpdateCurrentColorFromHslSlider(s: ((SliderObject)sender).Value);
            SaturationSliderObject.Freeze = false;
        }

        public void OnLightnessValueChanged(object sender)
        {
            LightnessSliderObject.Freeze = true;
            UpdateCurrentColorFromHslSlider(l: ((SliderObject)sender).Value);
            LightnessSliderObject.Freeze = false;
        }

        public void On_ColorObjectColor_Changed(object sender)
        {
            ColorObject colorObject = (ColorObject)sender;

            //If changes not triggered by a slider
            if (!HueSliderObject.Freeze && !SaturationSliderObject.Freeze && !LightnessSliderObject.Freeze)
            {
                Update_HslSlidersValue_FromBrush(colorObject);
            }
            Update_HslSlidersBrush_FromBrush(colorObject);
        }

        public void Update_HslSlidersValue_FromBrush(ColorObject colorObject)
        {
            this.HueSliderObject.SetValue(ColorMethods.Get_H(colorObject.SolidColorBrush.Color));
            this.SaturationSliderObject.SetValue(ColorMethods.Get_S(colorObject.SolidColorBrush.Color));
            this.LightnessSliderObject.SetValue(ColorMethods.Get_L(colorObject.SolidColorBrush.Color));
        }

        public void Update_HslSlidersBrush_FromBrush(ColorObject colorObject)
        {
            this.HueSliderObject.SetBrush(colorObject.SolidColorBrush);
            this.SaturationSliderObject.SetBrush(colorObject.SolidColorBrush);
            this.LightnessSliderObject.SetBrush(colorObject.SolidColorBrush);
        }

        public void SetCurrentColorBrush(SolidColorBrush SolidColorBrush)
        {
            this.CurrentColor.SetBrush(new SolidColorBrush(SolidColorBrush.Color));
        }

        private void UpdateCurrentColorFromHslSlider(object h = null, object s = null, object l = null)
        {
            double hueValue = (double)(h ?? this.CurrentColor.SolidColorBrush.Color.Get_H());
            double saturationValue = (double)(s ?? this.CurrentColor.SolidColorBrush.Color.Get_S());
            double lightnessValue = (double)(l ?? this.CurrentColor.SolidColorBrush.Color.Get_L());

            if (hueValue == 1) hueValue = 0.999;
            if (saturationValue == 1) saturationValue = 0.999;
            if (lightnessValue == 1) lightnessValue = 0.999;

            Color color = ColorMethods.HSL2RGB(hueValue, saturationValue, lightnessValue);
            color.A = this.CurrentColor.SolidColorBrush.Color.A;
            SetCurrentColorBrush(new SolidColorBrush(color));
        }
    }
}