using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;
using System.Threading;

namespace AxisAvaloniaApp.UserControls.Switcher
{
    public partial class Slider : UserControl
    {
        private Border lCont;
        private Border rCont;
        private Border glf;
        private double xStart;
        private double xFinish;
        private double xLimit;
        private double xMiddlePoint;
        private DoubleTransition doubleTransition;

        private bool isPointerPressedSub;
        private bool isPointerMovedSub;
        private bool isPointerRealisedSub;

        Stopwatch stopWatch;

        public static readonly DirectProperty<Slider, bool> IsCheckedProperty =
             AvaloniaProperty.RegisterDirect<Slider, bool>(
             nameof(IsChecked),
             o => o.IsChecked,
             (o, v) => o.IsChecked = v);
        private bool _IsChecked = false;
        public bool IsChecked
        {
            get { return _IsChecked; }
            set 
            {
                SetAndRaise(IsCheckedProperty, ref _IsChecked, value);
                if (glf.Transitions.IndexOf(doubleTransition) == -1)
                {
                    glf.Transitions.Add(doubleTransition);
                }
                if (IsChecked)
                {
                    Canvas.SetLeft(glf, xLimit - this.BorderThickness.Right + 1);
                }
                else
                {
                    Canvas.SetLeft(glf, -(this.Bounds.Width - GlyphWidth + this.BorderThickness.Left) + 1);
                }
            }
        }

        public static readonly DirectProperty<Slider, string> LeftContentKeyProperty =
              AvaloniaProperty.RegisterDirect<Slider, string>(
              nameof(LeftContentKey),
              o => o.LeftContentKey,
              (o, v) => o.LeftContentKey = v);
        private string _LeftContentKey = "strImport";
        public string LeftContentKey
        {
            get { return _LeftContentKey; }
            set 
            {
                 SetAndRaise(LeftContentKeyProperty, ref _LeftContentKey, value);
            }
        }

        public static readonly DirectProperty<Slider, string> RightContentKeyProperty =
              AvaloniaProperty.RegisterDirect<Slider, string>(
              nameof(RightContentKey),
              o => o.RightContentKey,
              (o, v) => o.RightContentKey = v);
        private string _RightContentKey = "strExport";
        public string RightContentKey
        {
            get { return _RightContentKey; }
            set
            {
                SetAndRaise(RightContentKeyProperty, ref _RightContentKey, value);
            }
        }

        public static readonly DirectProperty<Slider, double> GlyphWidthProperty =
             AvaloniaProperty.RegisterDirect<Slider, double>(
             nameof(GlyphWidth),
             o => o.GlyphWidth,
             (o, v) => o.GlyphWidth = v);
        private double _GlyphWidth = 20;
        public double GlyphWidth
        {
            get { return _GlyphWidth; }
            set
            {
                SetAndRaise(GlyphWidthProperty, ref _GlyphWidth, value);
            }
        }

        public Slider()
        {
            InitializeComponent();
            glf = this.FindControl<Border>("glyph");
            lCont = this.FindControl<Border>("LeftContent");
            rCont = this.FindControl<Border>("RightContent");

            glf.Transitions = new Transitions();
            doubleTransition = new DoubleTransition();
            doubleTransition.Duration = TimeSpan.FromSeconds(0.3);
            doubleTransition.Property = Canvas.LeftProperty;
            stopWatch = new Stopwatch();

            this.PropertyChanged += Slider_PropertyChanged;
        }

        private void Slider_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
    
            if (e.Property.Name == "Bounds" && this.Content != null)
            {   
                
                lCont.Width = this.Bounds.Width - GlyphWidth - this.BorderThickness.Left - this.BorderThickness.Right;
                rCont.Width = this.Bounds.Width - GlyphWidth - this.BorderThickness.Left - this.BorderThickness.Right;
                lCont.Height = this.Bounds.Height - this.BorderThickness.Top - this.BorderThickness.Bottom;
                rCont.Height = this.Bounds.Height - this.BorderThickness.Top - this.BorderThickness.Bottom;


                RecalcLimits();
                glf.Transitions.Clear();
                IsChecked = IsChecked;
            }
        }

        private void Glyph_PointerEnter(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            this.Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand);
            Debug.WriteLine("PointerPressed-sub");
            glf.PointerPressed += Glf_PointerPressed;
            isPointerPressedSub = true;
        }

        private void Glf_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            stopWatch.Start();

            glf.Transitions.Clear();
            Debug.WriteLine("PointerPressed-Raised");
            Point p = e.GetPosition(this);
            xStart = p.X;
            glf.PointerMoved += Glf_PointerMoved;
            Debug.WriteLine("PointerMoved-sub");
            isPointerMovedSub = true;
            Debug.WriteLine("PointerReleased-sub");
            glf.PointerReleased += Glf_PointerReleased;
            isPointerRealisedSub = true;
        }

        private void Glf_PointerMoved(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            glf.Transitions.Clear();
            Debug.WriteLine("Move-Raised");
            RecalcLimits();
            Point p = e.GetPosition(this);
            xFinish = p.X;
            double newLeft = Canvas.GetLeft(glf) - xStart + xFinish;
            if (newLeft > xLimit - this.BorderThickness.Right)
            {
                newLeft = xLimit - this.BorderThickness.Right;
            }
            if (newLeft < -(this.Bounds.Width - GlyphWidth + this.BorderThickness.Left))
            {
                newLeft = -(this.Bounds.Width - GlyphWidth + this.BorderThickness.Left);
            }

            Canvas.SetLeft(glf, newLeft);
            xStart = xFinish;
        }

        private void RecalcLimits()
        {
            xLimit = this.BorderThickness.Top + this.BorderThickness.Bottom;
            xMiddlePoint = (xLimit - (this.Bounds.Width - GlyphWidth)) / 2;
        }

        private void Glf_PointerReleased(object? sender, Avalonia.Input.PointerReleasedEventArgs e)
        {
            Unsubscribe();
            stopWatch.Stop();
            TakeDisigion();
            glf.PointerPressed += Glf_PointerPressed;
            isPointerPressedSub = true;
        }       

        private void Glyph_PointerExit(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            Debug.WriteLine("PointerExit-Raised");
            this.Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Arrow);
            Unsubscribe();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Unsubscribe()
        {
            if (isPointerMovedSub)
            {
                glf.PointerMoved -= Glf_PointerMoved;
                Debug.WriteLine("move-unsub");
                isPointerMovedSub = false;
            }
            if (isPointerPressedSub)
            {
                glf.PointerPressed -= Glf_PointerPressed;
                Debug.WriteLine("pressed-unsub");
                isPointerPressedSub = false;
            }
            if (isPointerRealisedSub)
            {
                glf.PointerReleased -= Glf_PointerReleased;
                Debug.WriteLine("released-unsub");
                isPointerRealisedSub = false;
            }
        }

        private void TakeDisigion()
        {
            Debug.WriteLine(stopWatch.ElapsedMilliseconds);
            if (stopWatch.ElapsedMilliseconds > 200)
            {
                SetNearestValue();               
            }
            else
            {
                SetOppositeValue();
            }
            stopWatch.Reset();
        }

        private void SetOppositeValue()
        {
            IsChecked = Canvas.GetLeft(glf) < xMiddlePoint;
        }

        private void SetNearestValue()
        {
            IsChecked = Canvas.GetLeft(glf) > xMiddlePoint;
        }
    }
}
