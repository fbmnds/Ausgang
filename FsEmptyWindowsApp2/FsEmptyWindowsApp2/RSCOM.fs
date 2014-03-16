module RSCOM 


open System.IO.Ports

type RSCOM (serialPort: string,
            baud:       int32,
            parity:     Parity,
            dataBits:   int,
            stopBits:   StopBits) =

    let mutable connection: SerialPort option = None
    
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/aa363254%28v=vs.85%29.aspx
    let CLRBREAK = 9
    let CLRDTR   = 6
    let CLRRTS   = 4
    let SETBREAK = 8
    let SETDTR   = 5
    let SETRTS   = 3
    let SETXOFF  = 1
    let SETXON   = 2
    
    let mutable serialPort_ = serialPort
    let mutable baud_ = baud
    let mutable parity_ = parity
    let mutable dataBits_ = dataBits
    let mutable stopBits_ = stopBits    

    let mutable RTS_ = false
    let mutable TXD_ = false

    member x.Connect() =
        try
            connection <- Some ( new SerialPort(serialPort_, baud_, parity_, dataBits_, stopBits_) )
            match connection with
            | Some connection -> connection.ReadTimeout <- 500;  connection.WriteTimeout <- 500
            | _ -> "Connect: not connected" |> System.Windows.MessageBox.Show |> ignore
        with
        | ex -> ex.ToString() |> System.Windows.MessageBox.Show |> ignore

    member x.Open() =
        try
            match connection with
            | Some connection -> connection.Open()
            | _ -> "Open: not connected" |> System.Windows.MessageBox.Show |> ignore
        with
        | ex -> ex.ToString() |> System.Windows.MessageBox.Show |> ignore

    member x.Close() =
        try
            match connection with
            | Some connection -> connection.Close()
            | _ -> "Close: not connected" |> System.Windows.MessageBox.Show |> ignore
        with
        | ex -> ex.ToString() |> System.Windows.MessageBox.Show |> ignore

    member x.RTS state = 
        try 
            match connection with
            | Some connection -> connection.RtsEnable <- state; RTS_ <- state; x.TXD TXD_ |> ignore
            | _ -> "RTS: not connected" |> System.Windows.MessageBox.Show |> ignore
            true
        with
        | ex -> ex.ToString() |> System.Windows.MessageBox.Show |> ignore; false
               
    // TODO: check performance issues
    // refer to http://www.hanselman.com/blog/PerformanceOfSystemIOPortsVersusUnmanagedSerialPortCode.aspx     
    member x.DTR state = 
        try
            match connection with
            | Some connection -> connection.DtrEnable <- state; 
                                 if not state then ((x.RTS RTS_, x.TXD TXD_) |> ignore)
            | _ -> "DTR: not connected" |> System.Windows.MessageBox.Show |> ignore
            true
        with
        | ex -> ex.ToString() |> System.Windows.MessageBox.Show |> ignore; false
 
    member x.TXD state = 
        try
            match connection with
            | Some connection -> connection.BreakState <- state; TXD_ <- state
            | _ -> "TXD: not connected" |> System.Windows.MessageBox.Show |> ignore
            true
        with
        | ex -> ex.ToString() |> System.Windows.MessageBox.Show |> ignore; false
 
        
    interface System.IDisposable with
        member x.Dispose() =
            try
                match connection with
                | Some connection -> if connection.IsOpen then connection.Close(); connection.Dispose()
                | _ -> "Dispose: not connected" |> System.Windows.MessageBox.Show |> ignore
            with
            | ex -> ex.ToString() |> System.Windows.MessageBox.Show |> ignore