Option Explicit On
Option Strict On

Imports System.Text
Module CrearFolio

    Public Function CrearFol(longitud As Integer) As String
        Dim caracteres As String = "1234567890"
        Dim res As New StringBuilder()
        Dim rnd As New Random()
        While 0 < System.Math.Max(System.Threading.Interlocked.Decrement(longitud), longitud + 1)
            res.Append(caracteres(rnd.[Next](caracteres.Length)))
        End While
        Return res.ToString
    End Function



End Module
