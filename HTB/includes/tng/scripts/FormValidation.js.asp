<!-- #include file="../../common/lib/Resources/KT_Resources.asp"-->
<%
	d = "tNG_FormValidation"
%>
//Javascript UniVAL Resources
UNI_Messages = {};
UNI_Messages['required'] 				= '<%= KT_escapeJS(KT_getResource("REQUIRED", d, null))%>';
UNI_Messages['type'] 					= '<%= KT_escapeJS(KT_getResource("TYPE", d, null))%>';
UNI_Messages['format'] 					= '<%= KT_escapeJS(KT_getResource("FORMAT", d, null))%>';
UNI_Messages['text_'] 					= '<%= KT_escapeJS(KT_getResource("TEXT_", d, null))%>';
UNI_Messages['text_email'] 				= '<%= KT_escapeJS(KT_getResource("TEXT_EMAIL", d, null))%>';
UNI_Messages['text_cc_generic']			= '<%= KT_escapeJS(KT_getResource("TEXT_CC_GENERIC", d, null))%>';
UNI_Messages['text_cc_visa'] 			= '<%= KT_escapeJS(KT_getResource("TEXT_CC_VISA", d, null))%>';
UNI_Messages['text_cc_mastercard'] 		= '<%= KT_escapeJS(KT_getResource("TEXT_CC_MASTERCARD", d, null))%>';
UNI_Messages['text_cc_americanexpress'] = '<%= KT_escapeJS(KT_getResource("TEXT_CC_AMERICANEXPRESS", d, null))%>';
UNI_Messages['text_cc_discover'] 		= '<%= KT_escapeJS(KT_getResource("TEXT_CC_DISCOVER", d, null))%>';
UNI_Messages['text_cc_dinersclub'] 		= '<%= KT_escapeJS(KT_getResource("TEXT_CC_DINERSCLUB", d, null))%>';
UNI_Messages['text_zip_generic'] 		= '<%= KT_escapeJS(KT_getResource("TEXT_ZIP_GENERIC", d, null))%>';
UNI_Messages['text_zip_us5'] 			= '<%= KT_escapeJS(KT_getResource("TEXT_ZIP_US5", d, null))%>';
UNI_Messages['text_zip_us9'] 			= '<%= KT_escapeJS(KT_getResource("TEXT_ZIP_US9", d, null))%>';
UNI_Messages['text_zip_canada'] 		= '<%= KT_escapeJS(KT_getResource("TEXT_ZIP_CANADA", d, null))%>';
UNI_Messages['text_zip_uk'] 			= '<%= KT_escapeJS(KT_getResource("TEXT_ZIP_UK", d, null))%>';
UNI_Messages['text_phone'] 				= '<%= KT_escapeJS(KT_getResource("TEXT_PHONE", d, null))%>';
UNI_Messages['text_ssn'] 				= '<%= KT_escapeJS(KT_getResource("TEXT_SSN", d, null))%>';
UNI_Messages['text_url'] 				= '<%= KT_escapeJS(KT_getResource("TEXT_URL", d, null))%>';
UNI_Messages['text_ip'] 				= '<%= KT_escapeJS(KT_getResource("TEXT_IP", d, null))%>';
UNI_Messages['text_color_hex'] 			= '<%= KT_escapeJS(KT_getResource("TEXT_COLOR_HEX", d, null))%>';
UNI_Messages['text_color_generic'] 		= '<%= KT_escapeJS(KT_getResource("TEXT_COLOR_GENERIC", d, null))%>';
UNI_Messages['numeric_'] 				= '<%= KT_escapeJS(KT_getResource("NUMERIC_", d, null))%>';
UNI_Messages['numeric_int'] 			= '<%= KT_escapeJS(KT_getResource("NUMERIC_INT", d, null))%>';
UNI_Messages['numeric_int_positive'] 	= '<%= KT_escapeJS(KT_getResource("NUMERIC_INT_POSITIVE", d, null))%>';
UNI_Messages['numeric_zip_generic'] 	= '<%= KT_escapeJS(KT_getResource("TEXT_ZIP_GENERIC", d, null))%>';
UNI_Messages['double_float'] 			= '<%= KT_escapeJS(KT_getResource("DOUBLE_FLOAT", d, null))%>';
UNI_Messages['double_float_positive'] 	= '<%= KT_escapeJS(KT_getResource("DOUBLE_FLOAT_POSITIVE", d, null))%>';
UNI_Messages['date_'] 					= '<%= KT_escapeJS(KT_getResource("DATE_", d, null))%>';
UNI_Messages['date_date'] 				= '<%= KT_escapeJS(KT_getResource("DATE_DATE", d, null))%>';
UNI_Messages['date_time'] 				= '<%= KT_escapeJS(KT_getResource("DATE_TIME", d, null))%>';
UNI_Messages['date_datetime'] 			= '<%= KT_escapeJS(KT_getResource("DATE_DATETIME", d, null))%>';
UNI_Messages['mask_'] 					= '<%= KT_escapeJS(KT_getResource("MASK_", d, null))%>';
UNI_Messages['regexp_'] 				= '<%= KT_escapeJS(KT_getResource("REGEXP_", d, null))%>';
UNI_Messages['text_min'] 				= '<%= KT_escapeJS(KT_getResource("TEXT_MIN", d, null))%>';
UNI_Messages['text_max'] 				=  '<%= KT_escapeJS(KT_getResource("TEXT_MAX", d, null))%>';
UNI_Messages['text_between'] 			= '<%= KT_escapeJS(KT_getResource("TEXT_BETWEEN", d, null))%>';
UNI_Messages['other_min'] 				= '<%= KT_escapeJS(KT_getResource("OTHER_MIN", d, null))%>';
UNI_Messages['other_max'] 				= '<%= KT_escapeJS(KT_getResource("OTHER_MAX", d, null))%>';
UNI_Messages['other_between'] 			= '<%= KT_escapeJS(KT_getResource("OTHER_BETWEEN", d, null))%>';
UNI_Messages['form_was_modified'] 		= '<%= KT_escapeJS(KT_getResource("FORM_WAS_MODIFIED", d, null))%>';