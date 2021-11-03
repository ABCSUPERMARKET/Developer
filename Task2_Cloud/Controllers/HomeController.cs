using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task2_Cloud.Models;
using Task2_Cloud.TableHandler;
using Task2_Cloud.BlobHandler;


namespace Task2_Cloud.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string id)
        {
            //for our edit
            if (!string.IsNullOrEmpty(id))
            {
                //set the name of the table
                TableManager TableManagerObj = new TableManager("Prod");
                //retrieve the car to be updated
                List<Prod> ProdListObj = TableManagerObj.RetrieveEntity<Prod>("RowKey eq '" + id +
                "'");
                Prod ProdObj = ProdListObj.FirstOrDefault();
                return View(ProdObj);
            }
            return View(new Prod());
        }
        // GET: Home
        [HttpPost]
        public ActionResult Index(string id, HttpPostedFileBase uploadFile, FormCollection
        formData)
        {
            Prod ProdObj = new Prod();
            ProdObj.prodName = formData["prodName"] == "" ? null : formData["prodName"];
            ProdObj.prodDesc = formData["prodDesc"] == "" ? null : formData["prodDesc"];
            double PPrice;
            if (double.TryParse(formData["prodPrice"], out PPrice))
            {
                ProdObj.prodPrice = double.Parse(formData["prodPrice"] == "" ? null :
                formData["prodPrice"]);
            }
            else
            {
                return View(new Prod());
            }
            foreach (string file in Request.Files)
            {
                uploadFile = Request.Files[file];
            }
            //blob container creation
            BlobManager BlobManagerObj = new BlobManager("pictures");
            string FileAbsoluteUri = BlobManagerObj.UploadFile(uploadFile);
            ProdObj.FilePath = FileAbsoluteUri.ToString();
            //Insert statement
            if (string.IsNullOrEmpty(id))
            {
                ProdObj.PartitionKey = "Prod";
                ProdObj.RowKey = System.Guid.NewGuid().ToString();
                TableManager TableManagerObj = new TableManager("Car");
                TableManagerObj.InsertEntity<Prod>(ProdObj, true);
            }
            else
            {
                ProdObj.PartitionKey = "Prod";
                ProdObj.RowKey = id;
                TableManager TableManagerObj = new TableManager("Prod");
                TableManagerObj.InsertEntity<Prod>(ProdObj, false);
            }
            return RedirectToAction("Get");
        }
        //get Cars
        public ActionResult Get()
        {
            TableManager TableManagerObj = new TableManager("Prod");
            List<Prod> PListObj = TableManagerObj.RetrieveEntity<Prod>(null);
            return View(PListObj);
        }
        //Delete Car
        public ActionResult Delete(string id)
        {
            //return the Car to be deleted
            TableManager TableManagerObj = new TableManager("Prod");
            List<Prod> PListObj = TableManagerObj.RetrieveEntity<Prod>("RowKey eq '" + id +
            "'");
           Prod ProdObj = PListObj.FirstOrDefault();
            //delete the Car
            TableManagerObj.DeleteEntity<Prod>(ProdObj);
            return RedirectToAction("Get");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}
 