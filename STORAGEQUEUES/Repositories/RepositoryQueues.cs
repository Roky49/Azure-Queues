using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STORAGE_QUEUES.Repositories
{
    public class RepositoryQueues
    {
        CloudQueueClient client;
        public RepositoryQueues()
        {
            //llamamos al nombre de la cuenta de almacenamiento en web.conf
            String keys = CloudConfigurationManager.GetSetting("cuentaazure");
            CloudStorageAccount account = CloudStorageAccount.Parse(keys);
            //Creamos un cliente para la cola
            this.client = account.CreateCloudQueueClient();

        }

        public CloudQueue Createcola(String name)
        {
            //Recuperamos el contenedor de la cola
            //de cada programador
            CloudQueue queue = this.client.GetQueueReference(name);
            //creamos la cola nueva
            queue.CreateIfNotExists();
            return queue;
        }

        public List<String> GetCloudQueue()
        {
            List<string> lista = new List<string>();

            //recuperamos la lista de todas las colas creadas
            IEnumerable<CloudQueue> queue = this.client.ListQueues();
           foreach(CloudQueue q in queue)
            {   // Guardamos el nombre de cada cola
                lista.Add(q.Name) ;
            }
            return lista;
        }

        public void CrearMensaje(String queuename, String mensaje)
        {
            //recuperamos la cola
            CloudQueue queue = this.Createcola(queuename);
           //añadimos nuestro mensaje a la lista de la cola
            CloudQueueMessage msj = new CloudQueueMessage(mensaje);
            queue.AddMessage(msj);

        }

        public List<CloudQueueMessage> GetMensajes(String queuename)
        {
            // creamos una lista para todos los mensajes de la cola
            List<CloudQueueMessage> mensajes = new List<CloudQueueMessage>();
            // recuperamos la cola
            CloudQueue queue = this.Createcola(queuename);
            
            foreach (CloudQueueMessage msj in queue.GetMessages(32))
            {  // guardamos los mensajes a la lista
                mensajes.Add(msj);
            }
            return mensajes;
        }

        public  void EliminarMensajes(String queuename)
        {
            // recuperamos la cola
            CloudQueue queue = this.Createcola(queuename);
            //elimino la cola
            queue.Delete();
        }
    }
}