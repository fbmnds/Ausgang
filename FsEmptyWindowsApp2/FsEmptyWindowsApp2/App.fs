module MainApp

open System
open System.Windows
open System.Windows.Controls

open FSharpx

open System.IO.Ports

type MainWindow = XAML<"MainWindow.xaml">


let x() =
    let t = new System.IO.Ports.SerialPort ("COM5",9600,Parity.None, 8, StopBits.One)
    t.set_DtrEnable true
    t.set_RtsEnable false    
 

let loadWindow() =
   let window = MainWindow()
   // Your awesome code code here and you have strongly typed access to the XAML via "window"
   let grey = new Media.SolidColorBrush( Media.Color.FromRgb (192uy, 192uy,192uy) )
   let red = new Media.SolidColorBrush( Media.Color.FromRgb (255uy, 0uy, 0uy) )
   let green = new Media.SolidColorBrush( Media.Color.FromRgb (0uy, 255uy, 0uy) )
   let r1 = window.RtsSignal :?> System.Windows.Shapes.Rectangle
   let r2 = window.DtrSignal :?> System.Windows.Shapes.Rectangle
   let r3 = window.TxdSignal :?> System.Windows.Shapes.Rectangle
   r1.Fill <- grey
   r2.Fill <- grey
   r3.Fill <- grey
   (window.RtsAn).Click.AddHandler(new System.Windows.RoutedEventHandler
    (fun sender e -> r1.Fill <- green) )
   (window.RtsAus).Click.AddHandler(new System.Windows.RoutedEventHandler
    (fun sender e -> r1.Fill <- red) )
   (window.DtrAn).Click.AddHandler(new System.Windows.RoutedEventHandler
    (fun sender e -> r2.Fill <- green) )
   (window.DtrAus).Click.AddHandler(new System.Windows.RoutedEventHandler
    (fun sender e -> r2.Fill <- red) )
   (window.TxdAn).Click.AddHandler(new System.Windows.RoutedEventHandler
    (fun sender e -> r3.Fill <- green) )
   (window.TxdAus).Click.AddHandler(new System.Windows.RoutedEventHandler
    (fun sender e -> r3.Fill <- red) )
   window.MainWindow




[<STAThread>]
    (new Application()).Run(loadWindow()) |> ignore