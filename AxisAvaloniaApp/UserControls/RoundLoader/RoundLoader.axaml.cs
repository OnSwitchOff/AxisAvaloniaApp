using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;

namespace AxisAvaloniaApp.UserControls.RoundLoader
{
    public class RoundLoader : ContentControl
    {
        private const int elipse1Angle = 135;
        private const int elipse2Angle = 180;
        private const int elipse3Angle = 225;
        private const int elipse4Angle = 270;
        private const int elipse5Angle = 315;
        private const int elipse6Angle = 0;
        private const int elipse7Angle = 45;
        private const int elipse8Angle = 90;

        /// <summary>
        /// 
        /// </summary>
        public RoundLoader()
        {
            LoaderColor = Brushes.Black;
            LoaderDiameter = 160;
            CircleDiameter = 40;
        }

        public static readonly StyledProperty<IBrush> LoaderColorProperty =
           AvaloniaProperty.Register<RoundLoader, IBrush>(nameof(LoaderColor));

        /// <summary>
        /// Color of loader.
        /// </summary>
        /// <date>16.05.2022.</date>
        public IBrush LoaderColor
        {
            get => GetValue(LoaderColorProperty);
            set => SetValue(LoaderColorProperty, value);
        }

        public static readonly StyledProperty<double> LoaderDiameterProperty =
           AvaloniaProperty.Register<RoundLoader, double>(nameof(LoaderDiameter));

        /// <summary>
        /// Diameter of loader.
        /// </summary>
        /// <date>16.05.2022.</date>
        public double LoaderDiameter
        {
            get => GetValue(LoaderDiameterProperty);
            set
            {
                SetValue(LoaderDiameterProperty, value);

                CircleDiameter = LoaderDiameter / 4;
            }
        }

        internal static readonly StyledProperty<double> CircleDiameterProperty =
           AvaloniaProperty.Register<RoundLoader, double>(nameof(CircleDiameter));

        /// <summary>
        /// Diameter of the element of loader.
        /// </summary>
        /// <date>16.05.2022.</date>
        internal double CircleDiameter
        {
            get => GetValue(CircleDiameterProperty);
            set
            {
                SetValue(CircleDiameterProperty, value);

                double radius = (LoaderDiameter - CircleDiameter) / 2;
                Elipse_1_Y = radius * (1 - Math.Sin(elipse1Angle / 180.0 * Math.PI));
                Elipse_1_X = radius * (1 + Math.Cos(elipse1Angle / 180.0 * Math.PI));

                Elipse_2_Y = radius * (1 - Math.Sin(elipse2Angle / 180.0 * Math.PI));
                Elipse_2_X = radius * (1 + Math.Cos(elipse2Angle / 180.0 * Math.PI));

                Elipse_3_Y = radius * (1 - Math.Sin(elipse3Angle / 180.0 * Math.PI));
                Elipse_3_X = radius * (1 + Math.Cos(elipse3Angle / 180.0 * Math.PI));

                Elipse_4_Y = radius * (1 - Math.Sin(elipse4Angle / 180.0 * Math.PI));
                Elipse_4_X = radius * (1 + Math.Cos(elipse4Angle / 180.0 * Math.PI));

                Elipse_5_Y = radius * (1 - Math.Sin(elipse5Angle / 180.0 * Math.PI));
                Elipse_5_X = radius * (1 + Math.Cos(elipse5Angle / 180.0 * Math.PI));

                Elipse_6_Y = radius * (1 - Math.Sin(elipse6Angle / 180.0 * Math.PI));
                Elipse_6_X = radius * (1 + Math.Cos(elipse6Angle / 180.0 * Math.PI));

                Elipse_7_Y = radius * (1 - Math.Sin(elipse7Angle / 180.0 * Math.PI));
                Elipse_7_X = radius * (1 + Math.Cos(elipse7Angle / 180.0 * Math.PI));

                Elipse_8_Y = radius * (1 - Math.Sin(elipse8Angle / 180.0 * Math.PI));
                Elipse_8_X = radius * (1 + Math.Cos(elipse8Angle / 180.0 * Math.PI));
            }
        }

        internal static readonly StyledProperty<double> Elipse_1_YProperty =
           AvaloniaProperty.Register<RoundLoader, double>(nameof(Elipse_1_Y));

        /// <summary>
        /// Position Y of the first elipse.
        /// </summary>
        /// <date>16.05.2022.</date>
        internal double Elipse_1_Y
        {
            get => GetValue(Elipse_1_YProperty);
            set => SetValue(Elipse_1_YProperty, value);
        }

        internal static readonly StyledProperty<double> Elipse_1_XProperty =
           AvaloniaProperty.Register<RoundLoader, double>(nameof(Elipse_1_X));

        /// <summary>
        /// Position X of the first elipse.
        /// </summary>
        /// <date>16.05.2022.</date>
        internal double Elipse_1_X
        {
            get => GetValue(Elipse_1_XProperty);
            set => SetValue(Elipse_1_XProperty, value);
        }

        internal static readonly StyledProperty<double> Elipse_2_YProperty =
           AvaloniaProperty.Register<RoundLoader, double>(nameof(Elipse_2_Y));

        /// <summary>
        /// Position Y of the second elipse.
        /// </summary>
        /// <date>16.05.2022.</date>
        internal double Elipse_2_Y
        {
            get => GetValue(Elipse_2_YProperty);
            set => SetValue(Elipse_2_YProperty, value);
        }

        internal static readonly StyledProperty<double> Elipse_2_XProperty =
           AvaloniaProperty.Register<RoundLoader, double>(nameof(Elipse_2_X));

        /// <summary>
        /// Position X of the second elipse.
        /// </summary>
        /// <date>16.05.2022.</date>
        internal double Elipse_2_X
        {
            get => GetValue(Elipse_2_XProperty);
            set => SetValue(Elipse_2_XProperty, value);
        }

        internal static readonly StyledProperty<double> Elipse_3_YProperty =
          AvaloniaProperty.Register<RoundLoader, double>(nameof(Elipse_3_Y));

        /// <summary>
        /// Position Y of the third elipse.
        /// </summary>
        /// <date>16.05.2022.</date>
        internal double Elipse_3_Y
        {
            get => GetValue(Elipse_3_YProperty);
            set => SetValue(Elipse_3_YProperty, value);
        }

        internal static readonly StyledProperty<double> Elipse_3_XProperty =
           AvaloniaProperty.Register<RoundLoader, double>(nameof(Elipse_3_X));

        /// <summary>
        /// Position X of the third elipse.
        /// </summary>
        /// <date>16.05.2022.</date>
        internal double Elipse_3_X
        {
            get => GetValue(Elipse_3_XProperty);
            set => SetValue(Elipse_3_XProperty, value);
        }

        internal static readonly StyledProperty<double> Elipse_4_YProperty =
           AvaloniaProperty.Register<RoundLoader, double>(nameof(Elipse_4_Y));

        /// <summary>
        /// Position Y of the fourth elipse.
        /// </summary>
        /// <date>16.05.2022.</date>
        internal double Elipse_4_Y
        {
            get => GetValue(Elipse_4_YProperty);
            set => SetValue(Elipse_4_YProperty, value);
        }

        internal static readonly StyledProperty<double> Elipse_4_XProperty =
           AvaloniaProperty.Register<RoundLoader, double>(nameof(Elipse_4_X));

        /// <summary>
        /// Position X of the fourth elipse.
        /// </summary>
        /// <date>16.05.2022.</date>
        internal double Elipse_4_X
        {
            get => GetValue(Elipse_4_XProperty);
            set => SetValue(Elipse_4_XProperty, value);
        }

        internal static readonly StyledProperty<double> Elipse_5_YProperty =
           AvaloniaProperty.Register<RoundLoader, double>(nameof(Elipse_5_Y));

        /// <summary>
        /// Position Y of the fifth elipse.
        /// </summary>
        /// <date>16.05.2022.</date>
        internal double Elipse_5_Y
        {
            get => GetValue(Elipse_5_YProperty);
            set => SetValue(Elipse_5_YProperty, value);
        }

        internal static readonly StyledProperty<double> Elipse_5_XProperty =
           AvaloniaProperty.Register<RoundLoader, double>(nameof(Elipse_5_X));

        /// <summary>
        /// Position X of the fifth elipse.
        /// </summary>
        /// <date>16.05.2022.</date>
        internal double Elipse_5_X
        {
            get => GetValue(Elipse_5_XProperty);
            set => SetValue(Elipse_5_XProperty, value);
        }

        internal static readonly StyledProperty<double> Elipse_6_YProperty =
           AvaloniaProperty.Register<RoundLoader, double>(nameof(Elipse_6_Y));

        /// <summary>
        /// Position Y of the sixth elipse.
        /// </summary>
        /// <date>16.05.2022.</date>
        internal double Elipse_6_Y
        {
            get => GetValue(Elipse_6_YProperty);
            set => SetValue(Elipse_6_YProperty, value);
        }

        internal static readonly StyledProperty<double> Elipse_6_XProperty =
           AvaloniaProperty.Register<RoundLoader, double>(nameof(Elipse_6_X));

        /// <summary>
        /// Position X of the sixth elipse.
        /// </summary>
        /// <date>16.05.2022.</date>
        internal double Elipse_6_X
        {
            get => GetValue(Elipse_6_XProperty);
            set => SetValue(Elipse_6_XProperty, value);
        }

        internal static readonly StyledProperty<double> Elipse_7_YProperty =
           AvaloniaProperty.Register<RoundLoader, double>(nameof(Elipse_7_Y));

        /// <summary>
        /// Position Y of the seventh elipse.
        /// </summary>
        /// <date>16.05.2022.</date>
        internal double Elipse_7_Y
        {
            get => GetValue(Elipse_7_YProperty);
            set => SetValue(Elipse_7_YProperty, value);
        }

        internal static readonly StyledProperty<double> Elipse_7_XProperty =
           AvaloniaProperty.Register<RoundLoader, double>(nameof(Elipse_7_X));

        /// <summary>
        /// Position X of the seventh elipse.
        /// </summary>
        /// <date>16.05.2022.</date>
        internal double Elipse_7_X
        {
            get => GetValue(Elipse_7_XProperty);
            set => SetValue(Elipse_7_XProperty, value);
        }

        internal static readonly StyledProperty<double> Elipse_8_YProperty =
           AvaloniaProperty.Register<RoundLoader, double>(nameof(Elipse_8_Y));

        /// <summary>
        /// Position Y of the eighth elipse.
        /// </summary>
        /// <date>16.05.2022.</date>
        internal double Elipse_8_Y
        {
            get => GetValue(Elipse_8_YProperty);
            set => SetValue(Elipse_8_YProperty, value);
        }

        internal static readonly StyledProperty<double> Elipse_8_XProperty =
           AvaloniaProperty.Register<RoundLoader, double>(nameof(Elipse_8_X));

        /// <summary>
        /// Position X of the eighth elipse.
        /// </summary>
        /// <date>16.05.2022.</date>
        internal double Elipse_8_X
        {
            get => GetValue(Elipse_8_XProperty);
            set => SetValue(Elipse_8_XProperty, value);
        }
    }
}
