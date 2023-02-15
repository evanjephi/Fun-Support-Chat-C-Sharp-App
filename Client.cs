/*
 * A client example using sockets in c# based upon Microsoft sample code
 * https://docs.microsoft.com/en-us/dotnet/framework/network-programming/synchronous-client-socket-example
 * 
 * File: client_sample.cs
 * 
 * Written by: Microsoft and updated by Mike Audet
 * 
 * Purpose: provide sample code for using sockets for assignment 1, COIS 3040
 *
 * Uasage: run this after running the server code
 * 
 * Description of parameters: none
 * 
 * Namespaces required: see using list below.
 */

//required
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

/*
 * SynchronousSocketClient
 * 
 * purpose: class encapsulating the actual client code
 * 
 * written by Microsoft and updated by Mike Audet
 * 
 * encapsulates client code and program code
 * 
 */
public class SynchronousSocketClient
{
    /*
    * StartClient()
    * 
    * purpose: The actual client code
    * 
    * written by Microsoft and updated by Mike Audet
    * 
    * Purpose: creates a socket and sends data to the server.
    * Sent data is echoed back from the server.
    * 
    * params: none:
    * 
    * returns: void
    */
    public static void StartClient()
    {
        // Data buffer for incoming data (from the server).  
        byte[] bytes = new byte[1024];

        // Connect to a remote device.  
        try
        {
            // Establish the remote endpoint for the socket.  
            // This example uses port 11000 on the local computer.
            // For our purposes, Dns.GetHostName() can be replaced by "localhost"
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

            //get the first IP address from the list
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            //combine  our ip address and port into an object
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP  socket.  
            Socket sender = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.  
            try
            {
                //call the server
                sender.Connect(remoteEP);

                Console.WriteLine("Socket connected to {0}",
                    sender.RemoteEndPoint.ToString());
                //Greet user on this console
                Console.WriteLine("\nWelcome to Fun Support Chat! \nHow can I help! Ask me questions!");
                
                //This sends user input and receives response from the server
                while (true)
                {
                    int bytesSent, bytesRec;
                        //User input
                        string userInput = "";                       
                        userInput = Console.ReadLine();
                        
                        byte[] msg = Encoding.ASCII.GetBytes(userInput);
                        // Send the data through the socket.  
                        bytesSent = sender.Send(msg);

                        // Receive the response from the remote device.  
                        bytesRec = sender.Receive(bytes);

                        //print out out echoed text
                        Console.WriteLine("{0}",
                            Encoding.ASCII.GetString(bytes, 0, bytesRec));     
                }
            }//end inner try for sending data
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        }//end outer try for connection
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        //wait for user input
        Console.Read();
    }//end StartClient

    //entry point
    public static int Main(String[] args)
    {
        while (true)
        {
            //run our test - make sure server is running first!
            StartClient();
            return 0;
        }
    }
}//end class SynchronousSocketClient