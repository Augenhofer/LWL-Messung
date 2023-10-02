using System;
using System.Collections;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace LWL_Messung
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            string server = "10.130.35.73";
            TcpClient client = new TcpClient(server, 9974);
            Console.WriteLine("Client connected.");
            log.Info(DateTime.Now +" - TCP-Client connected.");

            using (SslStream sslStream = new SslStream(client.GetStream(), false,
                new RemoteCertificateValidationCallback(ValidateServerCertificate), null))
            {

                sslStream.AuthenticateAsClient(server);

                Console.WriteLine("Waiting for client message...");
                byte[] messsage = Encoding.UTF8.GetBytes("Hello from the client.<EOF>");

                sslStream.Flush();
                string messageData = ReadMessage(sslStream, log);

            }
            client.Close();
            // Create a request for the URL.

        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        static string ReadMessage(SslStream sslStream, log4net.ILog log)
        {
            // Read the  message sent by the client. 
            // The client signals the end of the message using the 
            // "<EOF>" marker.
            byte[] buffer = new byte[3200];
            StringBuilder messageData = new StringBuilder();
            int bytes = -1;

            XmlReader xmlFile;
            XmlDocument xmlDoc = new XmlDocument();
            DataSet ds = new DataSet("tbl");

            do
            {
                bytes = sslStream.Read(buffer, 0, buffer.Length);

                //sslStream.Flush();

                // Use Decoder class to convert from bytes to UTF8
                // in case a character spans two buffers.
                Decoder decoder = Encoding.UTF8.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);

                //messageData = new StringBuilder();
                messageData.Append(chars);

                //Console.Clear();

                if (messageData.ToString().IndexOf("</lios:LIOSXml>") != -1 && messageData.ToString().IndexOf("<?xml version=") != -1 && messageData.ToString().IndexOf("measData") != -1)
                {

                    string XMLstring = messageData.ToString();
                    XMLstring = XMLstring.Remove(0, XMLstring.IndexOf("<?xml version="));


                    File.WriteAllText(@"c:\temp\LWL-Messung-HO1.xml", XMLstring);

                    //XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(XMLstring);
                    try
                    {
                        //XmlReader xmlReader = new XmlNodeReader(doc);
                        ds.ReadXml(@"c:\temp\LWL-Messung-HO1.xml");
                        
                    }
                    catch { }
                    try
                    {                   
                        if (ds != null )
                        {

                            //string cmdString = "INSERT INTO [LWL_Steintemp_Avg_HO1] ([Clock] ,[Fibre],[FibreName],[Zone1],[Zone2],[Zone3],[Zone4],[Zone5],[Zone6],[Zone7],[Zone8],[Zone9],[Zone10],[Zone11],[Zone12] " +
                            //                   " ,[Zone13],[Zone14],[Zone15],[Zone16],[Zone17],[Zone18],[Zone19],[Zone20],[Zone21],[Zone22],[Zone23],[Zone24],[Zone25],[Zone26],[Zone27],[Zone28],[Zone29],[Zone30] " +
                            //                   " ,[Zone31],[Zone32],[Zone33],[Zone34],[Zone35],[Zone36],[Zone37],[Zone38],[Zone39],[Zone40],[Zone41],[Zone42],[Zone43],[Zone44],[Zone45],[Zone46],[Zone47],[Zone48]) " +
                            //                   " VALUES (@Clock, @Fibre, @FibreName, @Zone1, @Zone2, @Zone3, @Zone4, @Zone5, @Zone6, @Zone7, @Zone8, @Zone9, @Zone10, @Zone11, @Zone12 " +
                            //                   " , @Zone13, @Zone14, @Zone15, @Zone16, @Zone17, @Zone18, @Zone19, @Zone20, @Zone21, @Zone22, @Zone23, @Zone24, @Zone25, @Zone26, @Zone27, @Zone28, @Zone29, @Zone30 " +
                            //                   " , @Zone31, @Zone32, @Zone33, @Zone34, @Zone35, @Zone36, @Zone37, @Zone38, @Zone39, @Zone40, @Zone41, @Zone42, @Zone43, @Zone44, @Zone45, @Zone46, @Zone47, @Zone48)";

                            string cmdString = "INSERT INTO [LWL_Steintemp_Avg_HO1] ([Clock] ,[Fibre],[FibreName],[Zone1],[Zone2],[Zone3],[Zone4],[Zone5],[Zone6],[Zone7],[Zone8],[Zone9],[Zone10],[Zone11],[Zone12] " +
                                                " ,[Zone13],[Zone14],[Zone15],[Zone16],[Zone17],[Zone18],[Zone19],[Zone20],[Zone21],[Zone22],[Zone23],[Zone24],[Zone25],[Zone26],[Zone27],[Zone28],[Zone29],[Zone30] " +
                                                " ,[Zone31],[Zone32],[Zone33],[Zone34],[Zone35],[Zone36],[Zone37],[Zone38],[Zone39],[Zone40],[Zone41],[Zone42],[Zone43],[Zone44],[Zone45],[Zone46],[Zone47],[Zone48] " +
                                                " ,[Zone49],[Zone50],[Zone51],[Zone52],[Zone53],[Zone54],[Zone55],[Zone56],[Zone57],[Zone58],[Zone59],[Zone60],[Zone61],[Zone62],[Zone63],[Zone64],[Zone65],[Zone66] " +
                                                " ,[Zone67],[Zone68] ,[Zone69],[Zone70],[Zone71],[Zone72],[Zone73],[Zone74],[Zone75],[Zone76],[Zone77],[Zone78],[Zone79],[Zone80],[Zone81],[Zone82],[Zone83],[Zone84],[Zone85],[Zone86],[Zone87] " +
                                                " ,[Zone88],[Zone89],[Zone90],[Zone91],[Zone92],[Zone93],[Zone94]) " +
                                               " VALUES (@Clock, @Fibre, @FibreName, @Zone1, @Zone2, @Zone3, @Zone4, @Zone5, @Zone6, @Zone7, @Zone8, @Zone9, @Zone10, @Zone11, @Zone12 " +
                                               " , @Zone13, @Zone14, @Zone15, @Zone16, @Zone17, @Zone18, @Zone19, @Zone20, @Zone21, @Zone22, @Zone23, @Zone24, @Zone25, @Zone26, @Zone27, @Zone28, @Zone29, @Zone30 " +
                                               " , @Zone31, @Zone32, @Zone33, @Zone34, @Zone35, @Zone36, @Zone37, @Zone38, @Zone39, @Zone40, @Zone41, @Zone42, @Zone43, @Zone44, @Zone45, @Zone46, @Zone47, @Zone48 " +
                                               " , @Zone49, @Zone50, @Zone51, @Zone52, @Zone53, @Zone54, @Zone55, @Zone56, @Zone57, @Zone58, @Zone59, @Zone60, @Zone61, @Zone62, @Zone63, @Zone64, @Zone65, @Zone66 " +
                                               " , @Zone67, @Zone68, @Zone69, @Zone70, @Zone71, @Zone72, @Zone73, @Zone74, @Zone75, @Zone76, @Zone77, @Zone78, @Zone79, @Zone80, @Zone81, @Zone82, @Zone83, @Zone84, @Zone85, @Zone86 " +
                                               " , @Zone87 ,@Zone88, @Zone89, @Zone90, @Zone91, @Zone92, @Zone93, @Zone94)";

                            string connString = "Data Source=2217DBDO01; Initial Catalog=XPSDetailData;User ID=XPS_Detail; Password=!pwXPS_Detail;";

                            if (Convert.ToInt32(ds.Tables["MeasData"].Rows[0]["fibre"]) + 1 == 1)
                            {

                                using (SqlConnection conn = new SqlConnection(connString))
                                {
                                    using (SqlCommand comm = new SqlCommand())
                                    {
                                        comm.Connection = conn;
                                        comm.CommandText = cmdString;
                                        comm.Parameters.AddWithValue("@Clock", Convert.ToDateTime(ds.Tables["timeStamp"].Rows[0]["timeStamp_Text"]));
                                        comm.Parameters.AddWithValue("@Fibre", Convert.ToInt32(ds.Tables["MeasData"].Rows[0]["fibre"]) + 1);
                                        comm.Parameters.AddWithValue("@FibreName", ds.Tables["MeasData"].Rows[0]["fibreName"]).ToString();

                                        for (int i = 0; i < ds.Tables["Zone"].Rows.Count; i++)
                                        {
                                            //comm.Parameters.AddWithValue("@Zone" + (1 + i), Convert.ToDouble(ds.Tables["Zone"].Rows[i]["TAvg"].ToString().Replace(".", ",")));
                                            comm.Parameters.AddWithValue("@Zone" + (1 + i), Convert.ToDouble(ds.Tables["tMax"].Rows[i]["tMax_Text"].ToString().Replace(".", ",")));
                                        }

                                        try
                                        {
                                            conn.Open();
                                            comm.ExecuteNonQuery();
                                            Console.WriteLine("Zeile hinzugefügt:" + DateTime.Now);
                                            //log.Info(DateTime.Now + "XML-File:" + XMLstring);
                                            messageData = new StringBuilder();
                                        }
                                        catch (SqlException e)
                                        {
                                            Console.WriteLine("Error: Insert der Daten fehlgeschlagen:" + e.Message);
                                            // do something with the exception
                                            // don't hide it
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Info: Nicht Faser 1");
                                messageData = new StringBuilder();
                            }
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Error: Importiertes XML Fehlerhaft!");
                        log.Error(DateTime.Now + "XML-File:" + XMLstring);
                        //Console.WriteLine(XMLstring);
                        messageData = new StringBuilder();
                    }
                    //xmlFile.Dispose();
                    //messageData = new StringBuilder();
                    ds.Clear();
                    //break;
                }
                else
                {
                    //Console.WriteLine(messageData.ToString());


                }
            } while (bytes != 0);

            return messageData.ToString();
        }

        static string RemoveInvalidXmlChars(string text)
        {
            var validXmlChars = text.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray();
            return new string(validXmlChars);
        }
    }
}
