<%@ Page Language="VB"%>
<%@ import Namespace="System.IO" %>
<%@ import Namespace="System.Drawing" %>
<%@ import Namespace="System.Drawing.Imaging" %>
<%@ import Namespace="System.Drawing.Drawing2D" %>
<%@ import Namespace="System.Collections" %>
<%@ import Namespace="System.Globalization" %>


<script runat="server">
   Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
   
   			Dim UseSecurityCheck As Boolean
			UseSecurityCheck = True
   
   			If UseSecurityCheck Then	
				' autentification mecanism
				Dim SecurityCheckOK As Boolean 
				SecurityCheckOK = False
				Dim SecurityFailureReason As String
				SecurityFailureReason = "ASP_IMAGE_DOTNETCOMP_ERROR_REQUEST"
				If lcase(Request.ServerVariables("REQUEST_METHOD")) = "post" Then
					Dim PathToSecurityFile As String
					PathToSecurityFile = Request.Form("PathToSecurityFile")
					If len(PathToSecurityFile) = 0 Then 
						SecurityFailureReason = "ASP_IMAGE_DOTNETCOMP_ERROR_NO_SECURITY_FILE"
					Else
						Try
							Dim fi as New FileInfo(PathToSecurityFile)
							If Not fi.Exists Then
								SecurityFailureReason = "ASP_IMAGE_DOTNETCOMP_ERROR_SECURITY_FILE_NOTFOUND###" & PathToSecurityFile
							Else
								 Try
									 Dim sr As StreamReader = New StreamReader(PathToSecurityFile)
									 Dim line As String
									 line = sr.ReadLine()
									 If line is nothing
										SecurityFailureReason = "ASP_IMAGE_DOTNETCOMP_ERROR_INVALID_SECURITY_FILE###" & PathToSecurityFile
									 Else
										If trim(line) = Cstr(len(PathToSecurityFile)) Then
											SecurityCheckOK = true
										Else
											SecurityFailureReason = "ASP_IMAGE_DOTNETCOMP_ERROR_INVALID_SECURITY_FILE###" & PathToSecurityFile 
										End If	
									 End If
									 sr.Close()	
								Catch ex as Exception
										SecurityFailureReason = "ASP_IMAGE_DOTNETCOMP_ERROR_NO_READ_SECURITY_FILE###" & PathToSecurityFile
								End Try	 
							End If							
						Catch ex as Exception
							SecurityFailureReason = "ASP_IMAGE_DOTNETCOMP_ERROR_SECURITY_FILE_NOTFOUND###" & PathToSecurityFile
						End Try
					End If
				End If
				
				If Not SecurityCheckOK Then
				  Response.write (SecurityFailureReason & Request.QueryString("command"))		
				  Response.End()
				End If
			Else
				Dim SecurityFailureReason As String
				SecurityFailureReason = "Request type is not POST"
				If lcase(Request.ServerVariables("REQUEST_METHOD")) <> "post" Then
					Response.write (SecurityFailureReason & Request.QueryString("command"))		
					Response.End()				
				End If
			End If			
			
			
 			Dim Command As String
			Command	= Request.Form("command")
			Select Case Command
	            ' --------------------------------------------
   				Case "imagesize"
                    Dim source As String
					Dim size(1) As Integer 
					
                    source = Request.Form ("source")
                    Try
                        size = ImageUtil.ImageSize(source)    
						Response.write (size(0) & "###" & size(1))
			        Catch ex as Exception
						If ex.Message() = "ErrorLoadingImage" Then
							Response.write ("ASP_IMAGE_DOTNETCOMP_ERROR_LOADING_IMAGE###" & Command & "##" & source)
							Response.End()
						End If
				        Response.write("ASP_IMAGE_DOTNETCOMP_ERROR_GENERIC###" & Command & "##" & ex.Message())
				        Response.end()
			        End Try		
			
                ' --------------------------------------------
   				Case "resize"
                    Dim source As String
                    Dim destination As String
                    Dim width As Integer
                    Dim height As Integer
                    Dim keep As Boolean


	                source = Request.Form ("source")
                    destination = Request.Form ("destination")
                    Try
                        width = Cint(Request.Form("width"))
                    Catch ex As Exception
                        ' throw an Error
                        Response.Write("ASP_IMAGE_DOTNETCOMP_ERROR_WIDTH###" & Command)
                        Response.End()
                    End Try

                    Try
                        height = Cint(Request.Form("height"))
                    Catch ex As Exception
                        ' throw an Error
                        Response.Write("ASP_IMAGE_DOTNETCOMP_ERROR_HEIGHT###" & Command)
                        Response.End()
                    End Try

                    keep = False
                    If LCase(Request.Form("keepproportion")) = "true" Then
                        keep = True
                    End If



                    Try
                        ImageUtil.Resize (source, destination, width, height, keep)    
			        Catch ex as Exception
						If ex.Message() = "ErrorLoadingImage" Then
							Response.write ("ASP_IMAGE_DOTNETCOMP_ERROR_LOADING_IMAGE###" & Command & "##" & source)
							Response.End()
						End If
						If ex.Message() = "ErrorSavingImage" Then
							Response.write ("ASP_IMAGE_DOTNETCOMP_ERROR_SAVING_IMAGE###" & Command & "##" & destination)
							Response.End()
						End If
				        Response.write("ASP_IMAGE_DOTNETCOMP_ERROR_GENERIC###" & Command & "##" & ex.Message())
				        Response.end()
			        End Try

                ' --------------------------------------------
				Case "crop"
                    Dim source As String
                    Dim destination As String
                    Dim x As Integer
                    Dim y As Integer
                    Dim width As Integer
                    Dim height As Integer
                    
                    source = Request.Form ("source")
                    destination = Request.Form ("destination")

                    Try
                        x = Cint(Request.Form("x"))
                    Catch ex As Exception
                        ' throw an Error
                        Response.Write("ASP_IMAGE_DOTNETCOMP_ERROR_X###" & Command)
                        Response.End()
                    End Try

                    Try
                        y = Cint(Request.Form("y"))
                    Catch ex As Exception
                        ' throw an Error
                       Response.Write("ASP_IMAGE_DOTNETCOMP_ERROR_Y###" & Command)
                        Response.End()
                    End Try

                    Try
                        width = Cint(Request.Form("width"))
                    Catch ex As Exception
                        ' throw an Error
                        Response.Write("ASP_IMAGE_DOTNETCOMP_ERROR_WIDTH###" & Command)
                        Response.End()
                    End Try

                    Try
                        height = Cint(Request.Form("height"))
                    Catch ex As Exception
                        ' throw an Error
                        Response.Write("ASP_IMAGE_DOTNETCOMP_ERROR_HEIGHT###" & Command)
                        Response.End()
                    End Try


                    If width <= 0 Or height <= 0 Or x < 0 Or y <0  Then
                        Response.Write("ASP_IMAGE_DOTNETCOMP_ERROR_NEGATIVE_DIMS###" & Command)
                        Response.End()
                    End If

                    Try
                        ImageUtil.Crop (source, destination, x, y, width, height)    
			        Catch ex as Exception
						If ex.Message() = "ErrorLoadingImage" Then
							Response.write ("ASP_IMAGE_DOTNETCOMP_ERROR_LOADING_IMAGE###" & Command & "##" & source)
							Response.End()
						End If
						If ex.Message() = "ErrorSavingImage" Then
							Response.write ("ASP_IMAGE_DOTNETCOMP_ERROR_SAVING_IMAGE###" & Command & "##" & destination)
							Response.End()
						End If
				        Response.write("ASP_IMAGE_DOTNETCOMP_ERROR_GENERIC###" & Command & "##" & ex.Message())
				        Response.end()
			        End Try


                ' --------------------------------------------
				Case "adjustquality"
                    Dim source As String
                    Dim destination As String
                    Dim quality As Integer
                    
                    source = Request.Form ("source")
                    destination = Request.Form ("destination")

                    Try
                        quality = Cint(Request.Form("quality"))
                    Catch ex As Exception
                        ' throw an Error
                        Response.Write("ASP_IMAGE_DOTNETCOMP_ERROR_QUALITY###" & Command)
                        Response.End()
                    End Try

                    If quality <= 0 Or quality > 100 Then
                        Response.Write("ASP_IMAGE_DOTNETCOMP_ERROR_QUALITY_RANGE###" & Command)
                        Response.End()
                    End If

                    Try
                        ImageUtil.AdjustQuality (source, destination, quality)    
			        Catch ex as Exception
						If ex.Message() = "ErrorLoadingImage" Then
							Response.write ("ASP_IMAGE_DOTNETCOMP_ERROR_LOADING_IMAGE###" & Command & "##" & source)
							Response.End()
						End If
						If ex.Message() = "ErrorSavingImage" Then
							Response.write ("ASP_IMAGE_DOTNETCOMP_ERROR_SAVING_IMAGE###" & Command & "##" & destination)
							Response.End()
						End If
				        Response.write("ASP_IMAGE_DOTNETCOMP_ERROR_GENERIC###" & Command & "##" & ex.Message())
				        Response.end()
			        End Try


                ' --------------------------------------------
				Case "rotate"
                    Dim source As String
                    Dim destination As String
                    Dim degree As Integer
                    
                    source = Request.Form ("source")
                    destination = Request.Form ("destination")

                    Try
                        degree = Cint(Request.Form("degree"))
                    Catch ex As Exception
                        ' throw an Error
                        Response.Write("ASP_IMAGE_DOTNETCOMP_ERROR_DEGREE###" & Command)
                        Response.End()
                    End Try

                    If degree <> 90 and degree <> 180 and degree <> 270 Then
                        Response.Write("ASP_IMAGE_DOTNETCOMP_ERROR_DEGREE_RANGE###" & Command)
                        Response.End()
                    End If

                    Try
                        ImageUtil.Rotate (source, destination, degree)    
			        Catch ex as Exception
						If ex.Message() = "ErrorLoadingImage" Then
							Response.write ("ASP_IMAGE_DOTNETCOMP_ERROR_LOADING_IMAGE###" & Command & "##" & source)
							Response.End()
						End If
						If ex.Message() = "ErrorSavingImage" Then
							Response.write ("ASP_IMAGE_DOTNETCOMP_ERROR_SAVING_IMAGE###" & Command & "##" & destination)
							Response.End()
						End If
				        Response.write("ASP_IMAGE_DOTNETCOMP_ERROR_GENERIC###" & Command & "##" & ex.Message())
				        Response.end()
			        End Try

                ' --------------------------------------------
				Case "flip"
                    Dim source As String
                    Dim destination As String
                    Dim direction As String
                    
                    source = Request.Form ("source")
                    destination = Request.Form ("destination")
                    direction = Request.Form ("direction")

                    Try
                        ImageUtil.Flip (source, destination, direction)    
			        Catch ex as Exception
						If ex.Message() = "ErrorLoadingImage" Then
							Response.write ("ASP_IMAGE_DOTNETCOMP_ERROR_LOADING_IMAGE###" & Command & "##" & source)
							Response.End()
						End If
						If ex.Message() = "ErrorSavingImage" Then
							Response.write ("ASP_IMAGE_DOTNETCOMP_ERROR_SAVING_IMAGE###" & Command & "##" & destination)
							Response.End()
						End If
				        Response.write("ASP_IMAGE_DOTNETCOMP_ERROR_GENERIC###" & Command & "##" & ex.Message())
				        Response.end()
			        End Try

                Case else
					Response.write("ASP_IMAGE_DOTNETCOMP_ERROR_CMD_NOT_SUPPORTED###" & Command)
					Response.end()
			End Select
			
   End Sub 'Page_Load
</script>



<script runat="server">

        '*********************************************************************
        ' ImageUtil Class
        ' Provides static methods for image manipulation 
        '*********************************************************************

		Public Class ImageUtil
            Private Shared Sub t (Byval msg As String)
                Throw new Exception (msg)
            End Sub


            ' ==== Size ===================================
            Public Shared Function ImageSize(ByVal source As String) As Integer()
                
                Dim originalImg As System.Drawing.Image
                Try
                    originalImg = System.Drawing.Image.FromFile(source, True)
                Catch ex As Exception
                    t("ErrorLoadingImage")
                End Try

                Dim originalSize(1) As Integer
				originalSize(0) = originalImg.Width
				originalSize(1) = originalImg.Height 
                originalImg.Dispose()

				Return originalSize
			End Function


            ' ==== RESIZE ===================================
            Public Shared Sub Resize(ByVal source As String, _
                                     ByVal destination As String, _
                                     ByVal newWidth As Integer, _
                                     ByVal newHeight As Integer, _
                                     ByVal keep As Boolean )
                
                
                Dim originalImg As System.Drawing.Image
                Try
                    originalImg = System.Drawing.Image.FromFile(source, True)
                Catch ex As Exception
                    t("ErrorLoadingImage")
                End Try

                Dim srcWidth As Integer
                Dim srcHeight As Integer
                srcWidth = originalImg.Width
                srcHeight = originalImg.Height

                ' compute values for resizing
                Dim destWidth As Integer     
                Dim destHeight As Integer    

				Dim ratioWidth As Double
				Dim ratioHeight As Double

				If keep = True And ((newWidth <> 0 And srcWidth < newWidth) And (newHeight <> 0 And srcHeight < newHeight)) Then
					originalImg.Dispose()
					' just copy the source to destination
					Try
	            		File.Copy(source, destination)
					Catch ex As Exception
						t("ErrorSavingImage")
					End Try					
					Exit Sub
				End If

				If keep = True Then
					If newWidth <> 0 And newHeight <> 0 Then
						ratioWidth = Cdbl(srcWidth) / Cdbl(newWidth)
						ratioHeight = Cdbl(srcHeight) / Cdbl(newHeight)
						If ratioWidth < ratioHeight Then
							destWidth = Cint(srcWidth / ratioHeight)
							destHeight = newHeight
						Else
							destWidth = newWidth
							destHeight = Cint(srcHeight / ratioWidth) 
						End If
					Else
						If newWidth <> 0 Then
							ratioWidth = Cdbl(srcWidth) / Cdbl(newWidth)
							destWidth = newWidth 
							destHeight = Cint(srcHeight / ratioWidth)
						ElseIf newHeight <> 0 Then
							ratioHeight = Cdbl(srcHeight) / Cdbl(newHeight) 
							destHeight = newHeight 
							destWidth = Cint(srcWidth / ratioHeight) 
						Else
							destWidth = srcWidth
							destHeight = srcHeight
						End If
					End If
				Else
					destWidth = newWidth
					destHeight = newHeight
				End If



                ' create the resized image
                Dim resizedBmp As Bitmap = New Bitmap(originalImg, destWidth, destHeight)

                Dim originalFormat As ImageFormat = originalImg.RawFormat
                originalImg.Dispose()

                ' Save the file in the original format
                Try
                   If originalFormat.Equals(ImageFormat.Jpeg) Then
                        ' Save Jpeg to quality 100 
                        Dim eps As EncoderParameters = New EncoderParameters(1)
                        eps.Param(0) = New EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100)
                        Dim ici As ImageCodecInfo = GetEncoderInfo("image/jpeg")
                        resizedBmp.Save(destination, ici, eps)
                   Else
                        resizedBmp.Save(destination, originalFormat)    
                   End If
                Catch ex As Exception
					t("ErrorSavingImage")
                Finally
                    resizedBmp.Dispose()
                End Try
			End Sub



            ' ==== CROP  ===================================
            Public Shared Sub Crop(ByVal source As String, _
                                     ByVal destination As String, _
                                     ByVal x As Integer, _
                                     ByVal y As Integer, _
                                     ByVal width As Integer, _
                                     ByVal height As Integer)
                
                
                Dim originalImg As System.Drawing.Image
                Try
                    originalImg = System.Drawing.Image.FromFile(source, True)
                Catch ex As Exception
					t("ErrorLoadingImage")
                End Try

                Dim originalFormat As ImageFormat = originalImg.RawFormat
                

                Dim originalBmp As Bitmap =  New Bitmap (originalImg)
                originalImg.Dispose()

                Dim cropRec As Rectangle = New Rectangle (x, y, width, height)
                Dim cropBmp As Bitmap = New Bitmap(cropRec.Width, cropRec.Height, originalBmp.PixelFormat)

                Dim destRec As Rectangle = New Rectangle (0, 0, width, height)
                Dim cropGraph As Graphics
                cropGraph = Graphics.FromImage(cropBmp)
                cropGraph.DrawImage (originalBmp, destRec, cropRec.X, cropRec.Y, cropRec.Width, cropRec.Height,  GraphicsUnit.Pixel)
                originalBmp.Dispose()


                ' Save the file in the original format
                Try
                   If originalFormat.Equals(ImageFormat.Jpeg) Then
                        Dim eps As EncoderParameters = New EncoderParameters(1)
                        eps.Param(0) = New EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100)
                        Dim ici As ImageCodecInfo = GetEncoderInfo("image/jpeg")
                        cropBmp.Save(destination, ici, eps)
                   Else
                        cropBmp.Save(destination, originalFormat)    
                   End If
                Catch ex As Exception
					t("ErrorSavingImage")
                Finally
                    cropBmp.Dispose()
                End Try
			End Sub


            ' ==== ADJUST QUALITY  ===================================
            Public Shared Sub AdjustQuality(ByVal source As String, _
                                     ByVal destination As String, _
                                     ByVal quality As Integer)
                
                
                Dim originalImg As System.Drawing.Image
                Try
                    originalImg = System.Drawing.Image.FromFile(source, True)
                Catch ex As Exception
					t("ErrorLoadingImage")
                End Try

                Dim originalFormat As ImageFormat = originalImg.RawFormat
                
                If originalFormat.Equals(ImageFormat.Jpeg) Then
                    Dim originalBmp As Bitmap =  New Bitmap (originalImg)
                    originalImg.Dispose()
                    Try
                        ' Save Jpeg to quality 
                        Dim eps As EncoderParameters = New EncoderParameters(1)
                        eps.Param(0) = New EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality)
                        Dim ici As ImageCodecInfo = GetEncoderInfo("image/jpeg")
                        originalBmp.Save(destination, ici, eps)
                    Catch ex As Exception
						t("ErrorSavingImage")
                    Finally
                        originalBmp.Dispose()
                    End Try
                Else
                    ' dont' do anything
                    originalImg.Dispose()
                End If
			End Sub


            ' ==== ROTATE  ===================================
            Public Shared Sub Rotate(ByVal source As String, _
                                     ByVal destination As String, _
                                     ByVal degree As Integer)
                
                
                Dim originalImg As System.Drawing.Image
                Try
                    originalImg = System.Drawing.Image.FromFile(source, True)
                Catch ex As Exception
					t("ErrorLoadingImage")
                End Try

                Dim originalFormat As ImageFormat = originalImg.RawFormat
                
                Select Case degree
                    Case 90
                        originalImg.RotateFlip(RotateFlipType.Rotate90FlipNone)
                    Case 180
                        originalImg.RotateFlip(RotateFlipType.Rotate180FlipNone)
                    Case 270
                        originalImg.RotateFlip(RotateFlipType.Rotate270FlipNone)
                End Select

                Try
                    originalImg.Save(destination)    
                Catch ex As Exception
					t("ErrorSavingImage")
                Finally
                    originalImg.Dispose()
                End Try

			End Sub


          ' ==== ROTATE  ===================================
            Public Shared Sub Flip(ByVal source As String, _
                                     ByVal destination As String, _
                                     ByVal direction As String)
                
                
                Dim originalImg As System.Drawing.Image
                Try
                    originalImg = System.Drawing.Image.FromFile(source, True)
                Catch ex As Exception
					t("ErrorLoadingImage")
                End Try

                Dim originalFormat As ImageFormat = originalImg.RawFormat
                
                If LCase(direction) = "horizontal" Then
                    originalImg.RotateFlip(RotateFlipType.RotateNoneFlipX)
                Else
                    originalImg.RotateFlip(RotateFlipType.RotateNoneFlipY)
                End If

                Try
                    originalImg.Save(destination)    
                Catch ex As Exception
					t("ErrorSavingImage")
                Finally
                    originalImg.Dispose()
                End Try

			End Sub



            Private Shared Function GetEncoderInfo(ByVal mimeType As String) As ImageCodecInfo
                Dim j As Integer
                Dim encoders As ImageCodecInfo()
                encoders = ImageCodecInfo.GetImageEncoders()
                For j = 0 To encoders.Length
                    If encoders(j).MimeType = mimeType Then
                        Return encoders(j)
                    End If
                Next j
                Return Nothing
            End Function


        End Class
</script>







