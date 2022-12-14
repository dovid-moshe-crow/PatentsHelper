using System;
using System.Collections.Generic;
using System.IO;


namespace PatentsHelperOutlook
{
    public static class AutoCompleteEmails
    {
        const int PT_NULL = 0x0001;
        const int PT_I2 = 0x0002;//union data
        const int PT_I4 = 0x0003;//union data
        const int PT_FLOAT = 0x0004;//union data
        const int PT_DOUBLE = 0x0005;//union data
        const int PT_BOOLEAN = 0x000B;//union data
        const int PT_I8 = 0x0014;//union data
        const int PT_SYSTIME = 0x0040;//union data
        const int PT_ERROR = 0x000A;//union data

        const int PT_STRING8 = 0x001E;//Dynamic Value
        const int PT_CLSID = 0x0048;//Dynamic Value
        const int PT_BINARY = 0x0102;//Dynamic Value
        const int PT_MV_BINARY = 0x1102;//Dynamic Value
        const int PT_MV_STRING8 = 0x101E;//Dynamic Value
        const int PT_MV_UNICODE = 0x101F;//Dynamic Value
        const int PT_TSTRING = 0x001F;//Dynamic Value

        const int PT_CURRENCY = 0x0006;
        const int PT_APPTIME = 0x0007;
        const int PT_OBJECT = 0x000D;
        const int PT_SVREID = 0x00FB;
        const int PT_SRESTRICT = 0x00FD;
        const int PT_ACTIONS = 0x00FE;

        const int PR_NICK_NAME_W = 0x6001001f;
        const int PR_ENTRYID = 0x0FFF0102;
        const int PR_DISPLAY_NAME_W = 0x3001001F;
        const int PR_EMAIL_ADDRESS_W = 0x3003001F;
        const int PR_ADDRTYPE_W = 0x3002001F;
        const int PR_SEARCH_KEY = 0x300B0102;
        const int PR_SMTP_ADDRESS_W = 0x39FE001f;
        const int PR_DROPDOWN_DISPLAY_NAME_W = 0X6003001f;
        const int PR_NICK_NAME_WEIGHT = 0x60040003;

        public static List<string> GetEmails()
        {
            string path = GetAutoCompleteStreamFilePath();
            if (path == null)
            {
                return new List<string>();
            }

            List<string> emails = GetAutoCompleteEmails(path);

            if (emails == null)
            {
                return new List<string>();
            }
            return emails;
        }
        private static string GetAutoCompleteStreamFilePath()
        {
            try
            {
                string path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\Microsoft\Outlook\RoamCache";
                DirectoryInfo fileDir = new DirectoryInfo(path);
                FileInfo[] dirFiles = fileDir.GetFiles("Stream_Autocomplete*.dat");
                if (dirFiles.Length == 0)
                {
                    return null;
                }
                else
                {
                    string dist = $@"{Path.GetTempPath()}\{dirFiles[0].Name}";
                    File.Copy(dirFiles[0].FullName, dist, true);
                    return dist;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static List<string> GetAutoCompleteEmails(string path)
        {
            List<string> emails = new List<string>();
            try
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    BinaryReader br = new BinaryReader(fs);

                    //read metadata
                    br.ReadBytes(4);

                    
                    //major verion number should be 12
                    int majorVersionNumber = br.ReadInt32();
                    //minor verion number should be 0
                    int minorVersionNumber = br.ReadInt32();

                    //muber of emails
                    int numberOfRows = br.ReadInt32();

                    for (int rows = 0; rows < numberOfRows; rows++)
                    {
                        int numberOfProperties = br.ReadInt32();

                        for (int properties = 0; properties < numberOfProperties; properties++)
                        {
                            byte[] propertyLayout = br.ReadBytes(4);
                            int propertyTag = BitConverter.ToInt32(propertyLayout, 0);
                            int propertyType = BitConverter.ToInt16(propertyLayout, 0);

                            //read Reserved Data
                            br.ReadBytes(4);

                           

                            byte[] unionData = br.ReadBytes(8);

                            if (propertyType == PT_TSTRING)
                            {
                                int stringlen = br.ReadInt32();
                                byte[] str = br.ReadBytes(stringlen);
                                if (propertyTag == PR_DROPDOWN_DISPLAY_NAME_W)
                                {
                                    string email = GetPT_TSTRING(str);
                                    emails.Add(email);
                                }
                            }
                            else if (propertyType == PT_CLSID)
                            {
                                br.ReadBytes(16);
                            }
                            else if (propertyType == PT_STRING8 || propertyType == PT_BINARY)
                            {
                                int len = br.ReadInt32();
                                br.ReadBytes(len);
                            }
                            else if (propertyType == PT_MV_BINARY || propertyType == PT_MV_STRING8 || propertyType == PT_MV_UNICODE)
                            {
                                int arrayLen = br.ReadInt32();

                                for (int i = 0; i < arrayLen; i++)
                                {
                                    int len = br.ReadInt32();
                                    br.ReadBytes(len);
                                }
                            }
                        }
                    }
                    return emails;
                }
            }
            catch (Exception)
            {
                return emails;
            }
        }


        private static string GetPT_TSTRING(byte[] str)
        {
            char[] tstring = new char[str.Length / 2];
            for (int i = 0; i < tstring.Length; i++)
            {
                tstring[i] = BitConverter.ToChar(str, i * 2);
            }
            return new string(tstring).Trim('\0');
        }
    }
}
