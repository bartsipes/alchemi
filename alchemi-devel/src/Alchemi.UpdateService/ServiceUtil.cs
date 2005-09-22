using System;
using System.IO;
using Microsoft.Win32.Security;


//even though this class belongs to the Core namespace, we dont put it in the ManagerService project,
//and link it in the ExecutorService since we dont want a dependency on the Win32.Security dll in the core dll.
//this is so far used only in the Service projects.
//with .Net 2.0 this will be in the base-class-lib.
namespace Alchemi.Core.Utility
{
	/// <summary>
	/// Summary description for Util.
	/// </summary>
	public class ServiceUtil
	{
		public static Boolean CreateDir(String strSitePath, String strUserName) 
		{
			Boolean bOk;
			try 
			{
				Directory.CreateDirectory(strSitePath);
				SecurityDescriptor secDesc = SecurityDescriptor.GetFileSecurity(strSitePath, SECURITY_INFORMATION.DACL_SECURITY_INFORMATION);
				Dacl dacl = secDesc.Dacl;
				Sid sidUser = new Sid (strUserName);
 
				// allow: folder, subfolder and files
				// modify
				dacl.AddAce (new AceAccessAllowed (sidUser, AccessType.GENERIC_WRITE | AccessType.GENERIC_READ | AccessType.DELETE | AccessType.GENERIC_EXECUTE , AceFlags.OBJECT_INHERIT_ACE | AceFlags.CONTAINER_INHERIT_ACE));          

				// deny: this folder
				// write attribs
				// write extended attribs
				// delete
				// change permissions
				// take ownership
				DirectoryAccessType DAType = DirectoryAccessType.FILE_WRITE_ATTRIBUTES | DirectoryAccessType.FILE_WRITE_EA | DirectoryAccessType.DELETE | DirectoryAccessType.WRITE_OWNER | DirectoryAccessType.WRITE_DAC;
				AccessType AType = (AccessType)DAType;
				dacl.AddAce (new AceAccessDenied (sidUser, AType));
				secDesc.SetDacl(dacl);
				secDesc.SetFileSecurity(strSitePath, SECURITY_INFORMATION.DACL_SECURITY_INFORMATION);
				bOk = true;
			} 
			catch 
			{
				bOk = false;
			}
			return bOk;
		} /* CreateDir */

	}
}
