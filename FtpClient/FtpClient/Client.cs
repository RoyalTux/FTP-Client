using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace FtpClient
{
    public class Client
    {
        private string password;
        private string userName;
        private string uri;
        private int bufferSize = 1024;

        public bool Passive = true;
        public bool Binary = true;
        public bool EnableSsl = false;
        public bool Hash = false;

        public Client(string uri, string userName, string password)
        {
            this.uri = uri;
            this.userName = userName;
            this.password = password;
        }

        public string ChangeWorkingDirectory(string path)
        {
            uri = combine(uri, path);

            return PrintWorkingDirectory();
        }

        public string DeleteFile(string fileName)
        {
            FtpWebRequest request = CreateRequest(combine(uri, fileName), WebRequestMethods.Ftp.DeleteFile);

            return getStatusDescription(request);
        }

        public string DownloadFile(string source, string dest)
        {
            FtpWebRequest request = CreateRequest(source, WebRequestMethods.Ftp.DownloadFile);

            byte[] buffer = new byte[bufferSize];

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (FileStream fs = new FileStream(dest, FileMode.OpenOrCreate))
                    {
                        int readCount = stream.Read(buffer, 0, bufferSize);

                        while (readCount > 0)
                        {
                            if (Hash)
                                Console.Write("#");

                            fs.Write(buffer, 0, readCount);
                            readCount = stream.Read(buffer, 0, bufferSize);
                        }
                    }
                }

                return response.StatusDescription;
            }
        }

        public DateTime GetDateTimestamp(string fileName)
        {
            FtpWebRequest request = CreateRequest(combine(uri, fileName), WebRequestMethods.Ftp.GetDateTimestamp);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                return response.LastModified;
            }
        }

        public long GetFileSize(string fileName)
        {
            FtpWebRequest request = CreateRequest(combine(uri, fileName), WebRequestMethods.Ftp.GetFileSize);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                return response.ContentLength;
            }
        }

        public string[] ListDirectory()
        {
            List<string> list = new List<string>();

            FtpWebRequest request = CreateRequest(WebRequestMethods.Ftp.ListDirectory);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, true))
                    {
                        while (!reader.EndOfStream)
                        {
                            list.Add(reader.ReadLine());
                        }
                    }
                }
            }

            return list.ToArray();
        }

        public string[] ListDirectoryDetails()
        {
            List<string> list = new List<string>();

            FtpWebRequest request = CreateRequest(WebRequestMethods.Ftp.ListDirectoryDetails);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, true))
                    {
                        while (!reader.EndOfStream)
                        {
                            list.Add(reader.ReadLine());
                        }
                    }
                }
            }

            return list.ToArray();
        }

        public string MakeDirectory(string directoryName)
        {
            FtpWebRequest request = CreateRequest(combine(uri, directoryName), WebRequestMethods.Ftp.MakeDirectory);

            return getStatusDescription(request);
        }

        public string PrintWorkingDirectory()
        {
            FtpWebRequest request = CreateRequest(WebRequestMethods.Ftp.PrintWorkingDirectory);

            return getStatusDescription(request);
        }

        public string RemoveDirectory(string directoryName)
        {
            FtpWebRequest request = CreateRequest(combine(uri, directoryName), WebRequestMethods.Ftp.RemoveDirectory);

            return getStatusDescription(request);
        }

        public string Rename(string currentName, string newName)
        {
            FtpWebRequest request = CreateRequest(combine(uri, currentName), WebRequestMethods.Ftp.Rename);

            request.RenameTo = newName;

            return getStatusDescription(request);
        }

        public string UploadFile(string source, string destination)
        {
            FtpWebRequest request = CreateRequest(destination, WebRequestMethods.Ftp.UploadFile);

            using (Stream stream = request.GetRequestStream())
            {
                using (FileStream fileStream = File.Open(source, FileMode.Open))
                {
                    int num;

                    byte[] buffer = new byte[bufferSize];

                    while ((num = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        if (Hash)
                            Console.Write("#");

                        stream.Write(buffer, 0, num);
                    }
                }
            }

            return getStatusDescription(request);
        }

        public string UploadFileWithUniqueName(string source)
        {
            FtpWebRequest request = CreateRequest(WebRequestMethods.Ftp.UploadFileWithUniqueName);

            using (Stream stream = request.GetRequestStream())
            {
                using (FileStream fileStream = File.Open(source, FileMode.Open))
                {
                    int num;

                    byte[] buffer = new byte[bufferSize];

                    while ((num = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        if (Hash)
                            Console.Write("#");

                        stream.Write(buffer, 0, num);
                    }
                }
            }

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                return Path.GetFileName(response.ResponseUri.ToString());
            }
        }

        private FtpWebRequest CreateRequest(string method)
        {
            return CreateRequest(uri, method);
        }

        private FtpWebRequest CreateRequest(string uri, string method)
        {
            FtpWebRequest r = (FtpWebRequest)WebRequest.Create(uri);

            r.Credentials = new NetworkCredential(userName, password);
            r.Method = method;
            r.UseBinary = Binary;
            r.EnableSsl = EnableSsl;
            r.UsePassive = Passive;

            return r;
        }

        private string getStatusDescription(FtpWebRequest request)
        {
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                return response.StatusDescription;
            }
        }

        private string combine(string path1, string path2)
        {
            return Path.Combine(path1, path2).Replace("\\", "/");
        }
    }
}