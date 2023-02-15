/*
 * A server example using sockets in c# based upon Microsoft sample code
 * https://docs.microsoft.com/en-us/dotnet/framework/network-programming/synchronous-server-socket-example
 * 
 * File: server_sample.cs
 * 
 * Written by: Microsoft and updated by Mike Audet
 * 
 * Purpose: provide sample code for using sockets for assignment 1, COIS 3040
 *
 * Uasage: run this before running the client code.
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
 * SynchronousSocketListener
 * 
 * purpose: class encapsulating the actual server code
 * 
 * written by Microsoft and updated by Mike Audet
 * 
 * encapsulates server code and program code
 * 
 */
public class SynchronousSocketListener
{

    // Incoming data from the client.  
    public static string data = null;

    void displayJokeOne(string receiveInput)
    {
        if (receiveInput == "who is there" || receiveInput == "who is there?")
        {
            string replayuser = "says";
        }
    }

    /*
     * StartListening()
     * 
     * purpose: The actual server code
     * 
     * written by Microsoft and updated by Mike Audet
     * 
     * Purpose: creates a socket and listens for incoming connections.
     * Any incoming data is echoed back out the socket to the sender.
     * 
     * params: none:
     * 
     * returns: void
     */
    public static void StartListening()
    {
        // Data buffer for incoming data.  
        byte[] bytes = new Byte[1024];

        //a flag for our main loop
        bool keepRunning = true;

        //a flag for reading in bytes
        bool moreData = true;


        // Establish the local endpoint for the socket.  
        // Dns.GetHostName returns the name of the
        // host running the application.  
        // Dns.GetHostName() can be replaced with “localhost” for our purposes
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        //get the first ip address returned from the list of addresses
        IPAddress ipAddress = ipHostInfo.AddressList[0];

        //IPEndPoint class encapsulates an IP address and a port
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.ipendpoint?view=net-6.0
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
        // Create a TCP/IP socket for sending and receiving data
        Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        // Bind the socket to the local endpoint and
        // listen for incoming connections.  
        try
        {
            //sets the listener Socket to the selected IP and port	
            listener.Bind(localEndPoint);

            //Listen just tells the socket to listen for incoming connections.  
            //The parameter of 10 is how long the incoming queue should be
            //https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.socket.listen?view=net-6.0 
            listener.Listen(10);
            // Start listening for connections.  
            while (keepRunning)
            {
                Console.WriteLine("Waiting for a connection...");
                // Program is suspended while waiting for an incoming connection.  
                // Note that we get a new socket when the connection is accepted. 
                Socket handler = listener.Accept();

                //if we make it to here, we have an incoming connection to process
                data = null;
                Console.WriteLine("Connected");
                //keep looping until we get an EOF message
                moreData = true;

                //loop to get all our input
                while (moreData)
                {
                    //actually receive some bytes
                    int bytesRec = handler.Receive(bytes);

                    // Convert the bytes we received into a String
                    // The param1 is the bytes, param2 is an offset, and param3 is the length
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                    //stop looping if we get the EOF string
                    //if (data.IndexOf("<EOF>") > -1)
                    //{
                    //    moreData = false;
                    //}  //end if we got an EOL
                    byte[] msg;
                    if (data == "y")
                        {
                         
                            // Echo the data back to the client.  bytes
                            msg = Encoding.ASCII.GetBytes("Knock, Knock.");
                            //send our reply
                            handler.Send(msg);
                            //Receive data incoming
                          
                            bytesRec = handler.Receive(bytes);
                            data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                            if (data == "who is there")
                            {
                                msg = Encoding.ASCII.GetBytes("says");
                                //send our reply
                                handler.Send(msg);
                               
                                //check for received data 
                                if (data == "says who")
                                {
                                    msg = Encoding.ASCII.GetBytes("says me!\n\nWould you like to play another game?\ny=yes, n=quit");
                                    //send our reply
                                    handler.Send(msg);
                                }
                                else
                                {
                                    msg = Encoding.ASCII.GetBytes("Enter valid phrase. i.e says who\n");
                                    //send our reply
                                    handler.Send(msg);
                                }
                            }
                            else {
                                Console.WriteLine("Enter valid phrase. i.e who is there");
                                msg = Encoding.ASCII.GetBytes("Enter valid phrase. i.e who is there\n");
                                //send our reply
                                handler.Send(msg);
                            }
                        }

                    else if (data == "n")
                    {
                        moreData = false;
                        msg = Encoding.ASCII.GetBytes("\nGood Bye. Run app to play again\n");
                        //send our reply
                        handler.Send(msg);
                    }
                    else {
                        msg = Encoding.ASCII.GetBytes("\nEnter Valid Input (y or n)\n");
                        //send our reply
                        handler.Send(msg);
                    }


                } //end while true 

                
                // Show the data on the console.  
                Console.WriteLine("Text received : {0} = quite\n Run the client to play again", data);

                

                //clean up and shut down the new socket created for this connection
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }  //end try block
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }  //end catch block

        Console.WriteLine("\nPress ENTER to continue...");
        Console.Read();

    }//end StartListening

    //entry point
    public static int Main(String[] args)
    {
        //start the server
        StartListening();
        return 0;
    }//end Main

}//end Class SynchronousSocketListener
