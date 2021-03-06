﻿using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AtelierXNA
{
    static class Serialiseur
    {
        static MemoryStream Sérialiser(object ObjToSerialize)
        {
            MemoryStream streamMemoire = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                formatter.Serialize(streamMemoire, ObjToSerialize);
                Console.WriteLine("Sérialisation réussie");
            }

            catch (Exception e)
            {
                Console.WriteLine("Erreur dans la sérialisation de: " + e.ToString());
            }

            finally
            {
                if (streamMemoire != null)
                    streamMemoire.Close();
            }
            return streamMemoire;
        }

        static T Désérialiser<T>(MemoryStream streamMemoire)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                return (T)formatter.Deserialize(streamMemoire);
            }

            catch
            {
                return default(T);
            }

            finally
            {
                if (streamMemoire != null)
                    streamMemoire.Close();
            }
        }

        static public byte[] ObjToByteArray(object objToTransform)
        {
            return Sérialiser(objToTransform).GetBuffer();
        }

        static public T ByteArrayToObj<T>(byte[] byteArray)
        {
            MemoryStream streamMemoire = new MemoryStream(byteArray);
            return Désérialiser<T>(streamMemoire); 
        }
    }
}
