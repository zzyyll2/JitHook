' VB demo with inline IL
module m1
sub Main()
        Console.WriteLine("Hi!")
        Dim x As Integer
        x = 3

#If IL Then
        // Here's some inline IL
        ldloc x
        dup
        add
        stloc x
#End If
        Console.WriteLine(x)
    End Sub

end module
