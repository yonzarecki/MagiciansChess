using System;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.Networking.Proximity;

namespace MagiciansChessApp
{
    public class BluetoothConnection
    {
        #region Properties
        private string bluetoothName;
        public StreamSocket socket;
        public DataReader dataReader;
        public DataWriter dataWriter;
        public RfcommDeviceService service;
        #endregion

        #region Methods
        /// <summary>
        /// The constructor 
        /// </summary>
        /// <param name="name">The name of the bluetooth module</param>
        public BluetoothConnection(string name)
        {
            bluetoothName = name;
        }


        /// <summary>
        /// The function connects to the Bluetooth module
        /// </summary>
        /// <remarks>Notice that when you connect once,
        /// the module may not connect the second time.
        /// To solve it just reboot the device</remarks>
        /// <returns>A Task, just await it</returns>
        public async Task Connect()
        {
            try
            {
                var devices =
                      await DeviceInformation.FindAllAsync(
                        RfcommDeviceService.GetDeviceSelector(
                          RfcommServiceId.SerialPort));
                var device = devices.Single(x => x.Name == bluetoothName);
                service = await RfcommDeviceService.FromIdAsync(
                                                        device.Id);
                socket = new StreamSocket();
                await socket.ConnectAsync(
                     service.ConnectionHostName,
                     service.ConnectionServiceName);
                dataReader = new DataReader(socket.InputStream);
                dataWriter = new DataWriter(socket.OutputStream);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                socket.Dispose();
                socket = null;
                service.Dispose();
                service = null;
            }
        }

        /// <summary>
        /// A function for disconnection
        /// </summary>
        /// <returns>A Task, just await it</returns>
        public async Task Disconnect()
        {
            try
            {
                await socket.CancelIOAsync();
                socket.Dispose();
                socket = null;
                service.Dispose();
                service = null;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        /// <summary>
        /// The funciton send a message and then wait until the 
        /// Bluetooth module send a message back (One char only!). 
        /// </summary>
        /// <param name="msg">The message to send</param>
        /// <returns>A task, just await it</returns>
        public async Task<string> Send(string msg)
        {
            try
            {
                var writer = dataWriter;
                writer.WriteString(msg);
                if (writer == null)
                {
                    Debug.WriteLine("you have a problem with writer");
                }
                var store = writer.StoreAsync();
                return await Recieve();
            }
            catch (Exception ex)
            {
                if (socket != null)
                {
                    Debug.Write(ex.Message);
                }
                else
                {
                    Debug.Write("Socket is null");
                }

                return "Bad Output";
            }
        }


        /// <summary>
        /// The functions waits for the module to send data (one char) back
        /// </summary>
        /// <returns>A task, just await it</returns>
        public async Task<string> Recieve()
        {
            DataReader bluetoothReader = dataReader;
            var bytesToRead = await bluetoothReader.LoadAsync(1);
            var oneByte = bluetoothReader.ReadString(bytesToRead);
            return oneByte.ToString();
        }
        #endregion

    }
}
