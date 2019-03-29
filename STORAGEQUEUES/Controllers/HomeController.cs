using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage.Queue;
using STORAGE_QUEUES.Repositories;
using System;
using System.Collections.Generic;

namespace STORAGEQUEUES.Controllers
{
   
    public class HomeController : Controller
    {
        
        RepositoryQueues repo;
        List<CloudQueueMessage> mensajes;
        public HomeController()
        {
            this.repo = new RepositoryQueues();
        }

        public ActionResult Index()
        {
            //todos los nombres de las queue
            ViewBag.colas = this.repo.GetCloudQueue();

            return View();
        }

        [HttpPost]
        public ActionResult Index( String mensaje, String nombre)
        {
            //todos los nombres de las queue
            ViewBag.colas = this.repo.GetCloudQueue();

           //preguntamos si el campo viene vacio 
                 if(mensaje != "") { 

                    String subir =  mensaje + " " + DateTime.Now.ToShortTimeString();
                    this.repo.CrearMensaje(nombre, subir);
                 }
           // recibo todos los mensajes
            mensajes = this.repo.GetMensajes(nombre);
            
            return View(mensajes);
        }
       
        public ActionResult CrearNuevaCola()
        {
            //todos los nombres de las queue
            List<String> colas = this.repo.GetCloudQueue();
            return View(colas);
        }

        [HttpPost]
        public ActionResult CrearNuevaCola(String nombre)
        {
            //creo la nueva queue
            CloudQueue lista = this.repo.Createcola(nombre);

            //todos los nombres de las queue
            List<String> colas = this.repo.GetCloudQueue();

            return View(colas);
        }

        public ActionResult Deletecola(String cola)
        {
            //eliminamos una queue por el nombre
            this.repo.EliminarMensajes(cola);

            return RedirectToAction("CrearNuevaCola");
        }


    }
}