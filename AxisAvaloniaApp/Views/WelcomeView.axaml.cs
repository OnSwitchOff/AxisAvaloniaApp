using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.TextFormatting;
using System;

namespace AxisAvaloniaApp.Views
{
    public partial class WelcomeView : UserControl
    {
        public WelcomeView()
        {
            InitializeComponent();

            //Avalonia.Controls.Html.HtmlLabel label = this.FindControl<Avalonia.Controls.Html.HtmlLabel>("htmlLabel");
            //string appPath1 = @"D:\Projects\AxisUno_old\AxisUNO\AxisUNO\AxisUNO\AxisUNO.Shared\Assets\OperationSchema.png";
            //string appPath2 = @"D:\Projects\AxisUno_old\AxisUNO\AxisUNO\AxisUNO\AxisUNO.Shared\Assets\Logos\Application.png";
            //label.Text = string.Format(
            //    "<!DOCTYPE html>" +
            //    "<html>" +
            //    "<body>" +
            //    "<div style=\"text-align:center;\"><img src = {0}></div>" +
            //    "<main>" +
            //    "<h1 style=\"color: DodgerBlue; text-align:center;\">Благодарим Ви за избора на Axis avalonia app!</h1>" +
            //    "<p style=\"font-size:110%;\"><b>Axis avalonia app</b> е съвременна система за управление на бизнес процесите в магазин, кафе, ресторант, склад или друг търговски обект.</p>" +
            //    "<div style=\"font-size:110%;\">" +
            //        "<img src={1} style=\"float:left; margin: 7px 7px 7px 0;\" width=\"150\" height=\"150\">" +
            //        "В наших секретных лабораториях в рамках проекта & laquo; Пандора & raquo; разрабатывалось " +
            //        "психотропное оружие.В результате неудачного эксперимента большинство ученых, работавших " +
            //        "над прибором, подверглись воздействию психотропного излучения, и они, находясь в состоянии " +
            //        "аффекта, растащили прототип по деталям. Возможно, наши ученые до сих пор находятся в " +
            //        "состоянии аффекта." +
            //    "</div> " +
            //    "</main>" +
            //    "</body>" +
            //    "</html>",
            //    appPath1, appPath2);

            textBlock = this.FindControl<TextBlock>("testLabel");
            //textBlock.Text = "This <Span FontWeight=\"Bold\">is</Span> a";
            mainBlock = this.FindControl<TextBlock>("mainLabel");

            //this.PropertyChanged += czxcsd;
        }

        public void Click()
        {

        }

        public static readonly StyledProperty<string> NewLineTextProperty =
            AvaloniaProperty.Register<WelcomeView, string>(nameof(NewLineText));

        /// <summary>
        /// Flag indicating whether the panel is collapsed.
        /// </summary>
        /// <date>18.05.2022.</date>
        public string NewLineText
        {
            get => GetValue(NewLineTextProperty);
            set => SetValue(NewLineTextProperty, value);
        }

        private TextBlock textBlock;
        private TextBlock mainBlock;

        private void czxcsd(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            {
                case nameof(TextBlock.Bounds):
                    NewLineText = string.Empty;

                    var textLenght = mainBlock.Text.Length;
                    int runsLenght = 0;
                    foreach(var line in mainBlock.TextLayout.TextLines)
                    {
                        runsLenght += line.TextRange.Length;
                    }

                    string mainText = mainBlock.Text.Substring(0, runsLenght);
                    string overflowText = mainBlock.Text.Substring(runsLenght);

                    if (textBlock.TextLayout.TextLines.Count != mainBlock.TextLayout.TextLines.Count)
                    {                        
                        for (int i = mainBlock.TextLayout.TextLines.Count; i < textBlock.TextLayout.TextLines.Count; i++)
                        {
                            NewLineText += mainBlock.Text.Substring(textBlock.TextLayout.TextLines[i].TextRange.Start, textBlock.TextLayout.TextLines[i].TextRange.Length);
                            //foreach (var item in textBlock.TextLayout.TextLines[i].TextRuns)
                            //{
                            //    NewLineText += item.Text.ToString();
                            //}
                        }
                        
                    }
                    break;
                case nameof(TextBlock.TransformedBounds):
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine(e.Property.Name);
                    break;
            }
            
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
