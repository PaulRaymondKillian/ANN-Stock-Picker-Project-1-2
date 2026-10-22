Public Class Stock
    Private _ID As Integer
    Private _open As Double
    Private _high As Double
    Private _low As Double
    Private _close As Double
    Private _volume As Integer
    Private _adjClose As Double

    Public ReadOnly Property ID() As Integer
        Get
            Return _ID
        End Get
    End Property

    Public Property open() As Double
        Get
            Return _open
        End Get
        Set(ByVal value As Double)
            If value > 0 Then
                _open = value
            Else
                Throw New Exception("!!!")
            End If
        End Set
    End Property

    Public Property high() As Double
        Get
            Return _high
        End Get
        Set(ByVal value As Double)
            If value > 0 Then
                _high = value
            Else
                Throw New Exception("!!!")
            End If
        End Set
    End Property

    Public Property low() As Double
        Get
            Return _low
        End Get
        Set(ByVal value As Double)
            If value > 0 Then
                _low = value
            Else
                Throw New Exception("!!!")
            End If
        End Set
    End Property

    Public Property close() As Double
        Get
            Return _close
        End Get
        Set(ByVal value As Double)
            If value > 0 Then
                _close = value
            Else
                Throw New Exception("!!!")
            End If
        End Set
    End Property

    Public Property volume() As Integer
        Get
            Return _volume
        End Get
        Set(ByVal value As Integer)
            If value >= 0 Then  'Allow volume to be 0 -This could be a result of a low-volume stock or a non-trading day
                _volume = value
            Else
                Throw New Exception("!!!")
            End If
        End Set
    End Property

    Public Property adjClose() As Double
        Get
            Return _adjClose
        End Get
        Set(ByVal value As Double)
            If value > 0 Then
                _adjClose = value
            Else
                Throw New Exception("!!!")
            End If
        End Set
    End Property

    Public Sub New(ByVal ID As Integer)
        _ID = ID
        _open = -1
        _high = -1
        _low = -1
        _close = -1
        _volume = -1
        _adjClose = -1
    End Sub

    Public Function PercentChangeThatDay() As Double
        Return (close - open) / close
    End Function

    Public Function PercentChange(ByVal endTime As Stock)
        Return (endTime.close - open) / open
    End Function

    Public Sub AdjustVolume()
        _volume = _volume / 100
    End Sub

End Class

