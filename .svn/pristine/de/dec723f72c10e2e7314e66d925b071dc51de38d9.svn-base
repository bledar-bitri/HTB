<%
Class tNG_ImageUpload
	Public doresize
	Public resizeProportional
	Public resizeWidth
	Public resizeHeight
	
	Private Sub Class_Initialize()
		Set this = Me
		Set parent = new tNG_FileUpload
		parent.SetContextObject Me
	End Sub

	Private Sub Class_Terminate()

	End Sub


	Public Sub Init (ByRef tNG__param)
		parent.Init  tNG__param
		doresize = false
		resizeProportional = true
		resizeWidth = 0
		resizeHeight = 0
	End Sub


	Public Sub setResize(proportional, width, height)
		doresize = true
		resizeProportional = Cbool(proportional)
		On Error Resume Next
		resizeWidth = Cint(width)
		resizeHeight = Cint(height)
		On Error GoTo 0
	End Sub

	Public Sub deleteThumbnails(folder, oldName)
		If oldName <> "" Then
			Set path_info = KT_pathinfo(oldName)
			regexp_str = KT_preg_quote (path_info("filename")) & "_\d+x\d+"
			If path_info("extension") <> "" Then
				regexp_str = regexp_str & "\." & KT_preg_quote(path_info("extension"))
			End If
			Set folderObj = new KT_folder
			Set entry = folderObj.readFolder (folder, false)
			If not folderObj.hasError() Then
				Set fso = Server.CreateObject("Scripting.FileSystemObject")
				For each fl_i in entry("files")
					fname  = entry("files")(fl_i)("name") 
					If KT_preg_test(regexp_str, fname) Then
						On Error resume Next
						fso.DeleteFile folder & fname
						On Error GoTo 0
					End If
				Next
				Set fso  = nothing
			End If
		End If
	End Sub

	Public Function Execute()
		Set ret = parent.Execute()
		If ret is nothing And doresize And uploadedFileName <> "" Then
			Set ret = this.Resize()
		End If	
		Set Execute = ret
	End Function

	Public Function Resize()
		Set ret = nothing
		Set image = new KT_image
		image.setPreferedLib tNG_prefered_image_lib
		image.addCommand tNG_prefered_imagemagick_path

		image.Resize dynamicFolder & uploadedFileName, dynamicFolder, uploadedFileName, resizeWidth, resizeHeight, resizeProportional
		If image.hasError() Then
			arrError = image.getError
			Set errObj = new tNG_error
			errObj.Init  "IMG_RESIZE", array(), array(arrError(1))
			If dbFieldName <> "" Then
				errObj.addFieldError dbFieldName, "IMG_RESIZE", array()
			End If
			Set ret = errObj
		End If
		Set image = nothing
		Set Resize = ret
	End Function


	'===========================================
	' Inheritance
	'===========================================
	Public this
	Public parent

	Public Sub SetContextObject(ByRef objContext)
		Set this = objContext
		parent.SetContextObject objContext
	End Sub

	'  Inherited properties from tNG_FileUpload
	'-------------------------------------------
	' tNG	
	Public Property Get tNG	
		Set tNG	 = parent.tNG	
	End Property
	Public Property Set tNG	(ByRef tNG__par)
		Set parent.tNG	 = tNG__par
	End Property

	' fieldName
	Public Property Get fieldName
		fieldName = parent.fieldName
	End Property
	Public Property Let fieldName(fieldName__par)
		parent.fieldName = fieldName__par
	End Property

	' formFieldName
	Public Property Get formFieldName
		formFieldName = parent.formFieldName
	End Property
	Public Property Let formFieldName(formFieldName__par)
		parent.formFieldName = formFieldName__par
	End Property

	' dbFieldName
	Public Property Get dbFieldName
		dbFieldName = parent.dbFieldName
	End Property
	Public Property Let dbFieldName(dbFieldName__par)
		parent.dbFieldName = dbFieldName__par
	End Property

	' folder
	Public Property Get folder
		folder = parent.folder
	End Property
	Public Property Let folder(folder__par)
		parent.folder = folder__par
	End Property

	' maxSize
	Public Property Get maxSize
		maxSize = parent.maxSize
	End Property
	Public Property Let maxSize(maxSize__par)
		parent.maxSize = maxSize__par
	End Property

	' allowedExtensions
	Public Property Get allowedExtensions
		allowedExtensions = parent.allowedExtensions
	End Property
	Public Property Let allowedExtensions(allowedExtensions__par)
		parent.allowedExtensions = allowedExtensions__par
	End Property

	' rename
	Public Property Get rename
		rename = parent.rename
	End Property
	Public Property Let rename(rename__par)
		parent.rename = rename__par
	End Property

	' renameRule
	Public Property Get renameRule
		renameRule = parent.renameRule
	End Property
	Public Property Let renameRule(renameRule__par)
		parent.renameRule = renameRule__par
	End Property

	' uploadedFileName
	Public Property Get uploadedFileName
		uploadedFileName = parent.uploadedFileName
	End Property
	Public Property Let uploadedFileName(uploadedFileName__par)
		parent.uploadedFileName = uploadedFileName__par
	End Property

	' dynamicFolder
	Public Property Get dynamicFolder
		dynamicFolder = parent.dynamicFolder
	End Property
	Public Property Let dynamicFolder(dynamicFolder__par)
		parent.dynamicFolder = dynamicFolder__par
	End Property


	Public Property Get errObj	
		Set errObj	 = parent.errObj	
	End Property
	Public Property Set errObj	(ByRef errObj__par)
		Set parent.errObj	 = errObj__par
	End Property

	'  Inherited properties as Property
	'-------------------------------------------

	'  Inherited methods
	'-------------------------------------------

	Public Sub setFormFieldName (formFieldName__param)
		parent.setFormFieldName formFieldName__param
	End Sub

	Public Sub setDbFieldName (dbFieldName__param)
		parent.setDbFieldName dbFieldName__param
	End Sub

	Public Sub setFolder (folder__param)
		parent.setFolder folder__param
	End Sub

	Public Sub setMaxSize (maxSize__param)
		parent.setMaxSize maxSize__param
	End Sub

	Public Sub setAllowedExtensions (allowedExtensions__param)
		parent.setAllowedExtensions allowedExtensions__param
	End Sub

	Public Sub setRename (rename__param)
		parent.setRename rename__param
	End Sub

	Public Sub setRenameRule (renameRule__param)
		parent.setRenameRule renameRule__param
	End Sub

	Public Sub Rollback()
		parent.Rollback 
	End Sub


End Class
%>