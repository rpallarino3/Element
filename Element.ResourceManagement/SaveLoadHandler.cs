using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Element.Common.Data;

namespace Element.ResourceManagement
{
    public static class SaveLoadHandler
    {
        private const string PREF_DATA_FILE = "ElementPrefData.sav";
        private const string FILE_0_DATA_NAME = "ElementSave0.sav";
        private const string FILE_1_DATA_NAME = "ElementSave1.sav";
        private const string FILE_2_DATA_NAME = "ElementSave2.sav";

        private const string PREF_DATA_FILE_OLD = "ElementPrefData.sav.old";
        private const string FILE_0_DATA_NAME_OLD = "ElementSave0.sav.old";
        private const string FILE_1_DATA_NAME_OLD = "ElementSave1.sav.old";
        private const string FILE_2_DATA_NAME_OLD = "ElementSave2.sav.old";

        private const string PREF_DATA_FILE_NEW = "ElementPrefData.sav.new";
        private const string FILE_0_DATA_NAME_NEW = "ElementSave0.sav.new";
        private const string FILE_1_DATA_NAME_NEW = "ElementSave1.sav.new";
        private const string FILE_2_DATA_NAME_NEW = "ElementSave2.sav.new";

        private static IAsyncResult result;

        // these should be maintained at the level of which we loaded/saved them
        // i'll need to copy the save data to another object to manipulate within the game
        private static SaveData _file0Data;
        private static SaveData _file1Data;
        private static SaveData _file2Data;

        #region Load

        // this should be called on start up of the game
        public static void LoadFiles()
        {
            try
            {
                IAsyncResult device = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
                device.AsyncWaitHandle.WaitOne();
                StorageDevice storageDevice = StorageDevice.EndShowSelector(device);

                if (storageDevice.IsConnected && storageDevice != null)
                {
                    IAsyncResult result = storageDevice.BeginOpenContainer("Element Data", null, null);
                    result.AsyncWaitHandle.WaitOne();
                    StorageContainer container = storageDevice.EndOpenContainer(result);
                    result.AsyncWaitHandle.Close();

                    // eh should probably break this shit out incase an exception is thrown
                    _file0Data = LoadFile(container, FILE_0_DATA_NAME);

                    if (_file0Data == null)
                        _file0Data = LoadFile(container, FILE_0_DATA_NAME_OLD);

                    _file1Data = LoadFile(container, FILE_1_DATA_NAME);

                    if (_file1Data == null)
                        _file1Data = LoadFile(container, FILE_1_DATA_NAME_OLD);

                    _file2Data = LoadFile(container, FILE_2_DATA_NAME);

                    if (_file2Data == null)
                        _file2Data = LoadFile(container, FILE_2_DATA_NAME_OLD);

                    container.Dispose();
                }

            }
            catch
            {
                Console.WriteLine("ERROR! Exception loading files.");
            }
        }

        // this should be called when we want to load specific data from the exit menu?
        // this actually might not be useful at all?
        public static void RequestLoad(int index)
        {
            try
            {
                IAsyncResult device = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
                device.AsyncWaitHandle.WaitOne();
                StorageDevice storageDevice = StorageDevice.EndShowSelector(device);

                if (storageDevice.IsConnected && storageDevice != null)
                {
                    IAsyncResult result = storageDevice.BeginOpenContainer("Element Data", null, null);
                    result.AsyncWaitHandle.WaitOne();
                    StorageContainer container = storageDevice.EndOpenContainer(result);
                    result.AsyncWaitHandle.Close();

                    if (index == 0)
                    {
                        _file0Data = LoadFile(container, FILE_0_DATA_NAME);

                        if (_file0Data == null)
                            _file0Data = LoadFile(container, FILE_0_DATA_NAME_OLD);
                    }
                    else if (index == 1)
                    {
                        _file1Data = LoadFile(container, FILE_1_DATA_NAME);

                        if (_file1Data == null)
                            _file1Data = LoadFile(container, FILE_1_DATA_NAME_OLD);
                    }
                    else if (index == 2)
                    {
                        _file2Data = LoadFile(container, FILE_2_DATA_NAME);

                        if (_file2Data == null)
                            _file2Data = LoadFile(container, FILE_2_DATA_NAME_OLD);
                    }

                    container.Dispose();
                    Console.WriteLine("Load complete!");
                }
            }
            catch
            {
                Console.WriteLine("ERROR! LOAD FAILED!!!");
            }
        }

        private static SaveData LoadFile(StorageContainer container, string fileName)
        {
            try
            {
                if (!container.FileExists(fileName))
                    return null;

                try
                {
                    Stream stream = container.OpenFile(fileName, FileMode.Open);
                    var data = LoadFile<SaveData>(fileName, stream);
                    stream.Close();
                    return data;
                }
                catch
                {
                    Console.WriteLine("Error creating filestream for " + fileName);
                }

                return null;
            }
            catch
            {
                Console.WriteLine("ERROR! Exception loading file: " + fileName);
                return null;
            }
        }

        #endregion

        #region Save

        public static void RequestSave(int index, SaveData data) // do we use this data or the data in this class?
        {
            try
            {
                IAsyncResult device = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
                device.AsyncWaitHandle.WaitOne();
                StorageDevice storageDevice = StorageDevice.EndShowSelector(device);

                if (storageDevice.IsConnected && storageDevice != null)
                {
                    IAsyncResult result = storageDevice.BeginOpenContainer("Element Data", null, null);
                    result.AsyncWaitHandle.WaitOne();
                    StorageContainer container = storageDevice.EndOpenContainer(result);
                    result.AsyncWaitHandle.Close();
                    
                    if (index == 0)
                    {
                        CopyFile(container, FILE_0_DATA_NAME, FILE_0_DATA_NAME_OLD);
                        SaveFile(container, FILE_0_DATA_NAME_NEW, data);
                        CopyFile(container, FILE_0_DATA_NAME_NEW, FILE_0_DATA_NAME);
                        _file0Data = data;
                    }
                    else if (index == 1)
                    {
                        CopyFile(container, FILE_1_DATA_NAME, FILE_1_DATA_NAME_OLD);
                        SaveFile(container, FILE_1_DATA_NAME_NEW, data);
                        CopyFile(container, FILE_1_DATA_NAME_NEW, FILE_1_DATA_NAME);
                        _file1Data = data;
                    }
                    else if (index == 2)
                    {
                        CopyFile(container, FILE_2_DATA_NAME, FILE_2_DATA_NAME_OLD);
                        SaveFile(container, FILE_2_DATA_NAME_NEW, data);
                        CopyFile(container, FILE_2_DATA_NAME_NEW, FILE_2_DATA_NAME);
                        _file2Data = data;
                    }
                }
                Console.WriteLine("Save complete!");
            }
            catch
            {
                Console.WriteLine("ERROR! SAVE FAILED!!!");
            }

        }

        private static void SaveFile(StorageContainer container, string fileName, SaveData data)
        {
            try
            {
                Stream stream = container.CreateFile(fileName);
                SaveFile(data, stream);
                stream.Close();
            }
            catch
            {
                Console.WriteLine("Error creating filestream for " + fileName);
            }
            finally
            {
                container.Dispose();
            }
        }

        private static void CopyFile(StorageContainer container, string fileName, string newFileName)
        {
            if (!container.FileExists(fileName))
                return;

            if (container.FileExists(newFileName))
                container.DeleteFile(newFileName);

            Stream oldFile = container.OpenFile(fileName, FileMode.Open);
            Stream newFile = container.CreateFile(newFileName);
            oldFile.CopyTo(newFile);

            oldFile.Close();
            newFile.Close();

            container.DeleteFile(fileName);
        }

        #endregion

        #region Erase

        public static void EraseFile(int index)
        {
            try
            {
                IAsyncResult device = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
                device.AsyncWaitHandle.WaitOne();
                StorageDevice storageDevice = StorageDevice.EndShowSelector(device);

                if (storageDevice.IsConnected && storageDevice != null)
                {
                    IAsyncResult result = storageDevice.BeginOpenContainer("Element Data", null, null);
                    result.AsyncWaitHandle.WaitOne();
                    StorageContainer container = storageDevice.EndOpenContainer(result);
                    result.AsyncWaitHandle.Close();

                    if (index == 0)
                    {
                        if (container.FileExists(FILE_0_DATA_NAME))
                            container.DeleteFile(FILE_0_DATA_NAME);

                        if (container.FileExists(FILE_0_DATA_NAME_OLD))
                            container.DeleteFile(FILE_0_DATA_NAME_OLD);

                        if (container.FileExists(FILE_0_DATA_NAME_NEW))
                            container.DeleteFile(FILE_0_DATA_NAME_NEW);                        
                    }
                    else if (index == 1)
                    {
                        if (container.FileExists(FILE_1_DATA_NAME))
                            container.DeleteFile(FILE_1_DATA_NAME);

                        if (container.FileExists(FILE_1_DATA_NAME_OLD))
                            container.DeleteFile(FILE_1_DATA_NAME_OLD);

                        if (container.FileExists(FILE_1_DATA_NAME_NEW))
                            container.DeleteFile(FILE_1_DATA_NAME_NEW);                        
                    }
                    else if (index == 2)
                    {
                        if (container.FileExists(FILE_2_DATA_NAME))
                            container.DeleteFile(FILE_2_DATA_NAME);

                        if (container.FileExists(FILE_2_DATA_NAME_OLD))
                            container.DeleteFile(FILE_2_DATA_NAME_OLD);

                        if (container.FileExists(FILE_2_DATA_NAME_NEW))
                            container.DeleteFile(FILE_2_DATA_NAME_NEW);
                    }

                    container.Dispose();
                    Console.WriteLine("Erase complete!");
                }
            }
            catch
            {
                Console.WriteLine("ERROR! ERASE FAILED!!!");
            }
        }

        #endregion

        #region Preference Data

        public static void SavePreferenceData(PreferenceData preferenceData)
        {
            try
            {
                IAsyncResult device = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
                device.AsyncWaitHandle.WaitOne();
                StorageDevice storageDevice = StorageDevice.EndShowSelector(device);

                if (storageDevice.IsConnected && storageDevice != null)
                {
                    IAsyncResult result = storageDevice.BeginOpenContainer("Element Data", null, null);
                    result.AsyncWaitHandle.WaitOne();
                    StorageContainer container = storageDevice.EndOpenContainer(result);
                    result.AsyncWaitHandle.Close();

                    if (container.FileExists(PREF_DATA_FILE)) // maybe try to rename file
                    {
                        container.DeleteFile(PREF_DATA_FILE);
                    }

                    try
                    {
                        Stream stream = container.CreateFile(PREF_DATA_FILE);
                        SaveFile(preferenceData, stream);
                        stream.Close();
                    }
                    catch
                    {
                        Console.WriteLine("Error creating file stream for " + PREF_DATA_FILE);
                    }
                    
                    container.Dispose();
                }
                Console.WriteLine("Preference data save complete!");
            }
            catch
            {
                Console.WriteLine("ERROR! PREFERENCE SAVE FAILED!!!");
            }
        }

        public static PreferenceData LoadPreferenceData() // fix this to close the stream if file doesn't deserialize properly
        {
            try
            {
                IAsyncResult device = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
                device.AsyncWaitHandle.WaitOne();
                StorageDevice storageDevice = StorageDevice.EndShowSelector(device);

                if (storageDevice.IsConnected && storageDevice != null)
                {
                    IAsyncResult result = storageDevice.BeginOpenContainer("Element Data", null, null);
                    result.AsyncWaitHandle.WaitOne();
                    StorageContainer container = storageDevice.EndOpenContainer(result);
                    result.AsyncWaitHandle.Close();

                    if (!container.FileExists(PREF_DATA_FILE)) 
                    {
                        container.Dispose();
                        Console.WriteLine("No preference data found, creating new preference data...");
                        return null;
                    }

                    PreferenceData prefData = null;

                    try
                    {
                        Stream stream = container.OpenFile(PREF_DATA_FILE, FileMode.Open);
                        prefData = LoadFile<PreferenceData>(PREF_DATA_FILE, stream);
                        stream.Close();
                    }
                    catch
                    {
                        Console.WriteLine("Error creating file stream for " + PREF_DATA_FILE);
                    }
                    
                    container.Dispose();
                    Console.WriteLine("Preference load complete!");
                    return prefData;
                }
                return null;
            }
            catch
            {
                Console.WriteLine("ERROR! PREFERENCE LOAD FAILED!!!");
                return null;
            }
        }

        #endregion

        private static T LoadFile<T>(string fileName, Stream stream)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                var data = (T)serializer.Deserialize(stream);
                return data;
            }
            catch
            {
                Console.WriteLine("Error deserializing data of type " + typeof(T).ToString());
                return default(T);
            }
        }

        private static void SaveFile<T>(T data, Stream stream)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, data);
            }
            catch
            {
                Console.WriteLine("Error serializing data of type " + typeof(T).ToString());
            }
        }

        public static SaveData File0Data { get { return _file0Data; } }
        public static SaveData File1Data { get { return _file1Data; } }
        public static SaveData File2Data { get { return _file2Data; } }
    }
}
