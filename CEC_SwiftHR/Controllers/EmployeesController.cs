using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CEC_SwiftHR.Models;
using CEC_SwiftHR.Models.CustomAttributes;
using CEC_SwiftHR.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CEC_SwiftHR.Controllers
{
    public class EmployeesController : Controller
    {

        private NewEmployeeEntities db = new NewEmployeeEntities();

        // GET: Employees
        public ActionResult Index()
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var employees = db.Employees.Include(e => e.EmployeeStatus).Include(e => e.EmployeeStatus1);
            return View(employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            //if (Session["Login"] == null)
            //{
            //    return RedirectToAction("Index", "Login");
            //}
            ViewBag.EmployeeId = new SelectList(db.EmployeeStatuses, "EmployeeStatusId", "Name");
            ViewBag.EmployeeStatusesId = new SelectList(db.EmployeeStatuses, "EmployeeStatusId", "Name");
            ViewBag.City = new SelectList(db.Cities, "CityId", "Name");
            ViewBag.District = new SelectList(db.Districts, "DistrictId", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeViewModel employeeViewModel,
            HttpPostedFileBase PhotoPath)
        {
            //if (Session["Login"] == null)
            //{
            //    return RedirectToAction("Index", "Login");
            //}
            if (ModelState.IsValid)
            {
                string strPath = "";
                string newfilename = "";
                if (PhotoPath != null)
                {
                    int startindex = PhotoPath.ContentType.LastIndexOf('/') + 1;
                    string filetype = PhotoPath.ContentType.Substring(startindex); //取得副檔名
                    newfilename = Guid.NewGuid() + "." + filetype;
                    strPath = Request.PhysicalApplicationPath + @"images\Employees\" + newfilename;
                    PhotoPath.SaveAs(strPath);
                }

                // Address
                Address address = new Address();
                address.AddressId = Guid.NewGuid();
                address.CityId = Guid.Parse(employeeViewModel.CitySelectedValue);
                address.DistrictId = Guid.Parse(employeeViewModel.DistrictSelectedValue);
                address.AddressLine = employeeViewModel.AddressLine;
                db.Addresses.Add(address);
                db.SaveChanges();
                //Residential Address
                Address ResidentialAddress = new Address();
                ResidentialAddress.AddressId = Guid.NewGuid();
                ResidentialAddress.CityId = Guid.Parse(employeeViewModel.ResidentialCitySelectedValue);
                ResidentialAddress.DistrictId = Guid.Parse(employeeViewModel.ResidentialDistrictSelectedValue);
                ResidentialAddress.AddressLine = employeeViewModel.ResidentialAddressLine;
                db.Addresses.Add(ResidentialAddress);
                db.SaveChanges();
                //Employee
                Employee emp = new Employee();
                emp.EmployeeId = Guid.NewGuid();
                emp.EmployeeName = employeeViewModel.EmployeeName;
                emp.EmployeeNameEn = employeeViewModel.EmployeeNameEn;
                emp.BirthDate = employeeViewModel.BirthDate;
                emp.IdCardNum = employeeViewModel.IdCardNum;
                emp.Gender = (int)employeeViewModel.Gender == 1;
                emp.BloodType = employeeViewModel.BloodType;
                emp.MobilePhone = employeeViewModel.MobilePhone;
                emp.PermanentAddressId = address.AddressId;
                emp.ResidentialAddressId = ResidentialAddress.AddressId;
                emp.Email = employeeViewModel.Email;
                emp.PermanentTel = employeeViewModel.PermanentTel;
                emp.ResidentialTel = employeeViewModel.ResidentialTel;
                emp.PhotoPath = newfilename;
                emp.OnBoardDate = employeeViewModel.OnBoardDate;
                emp.EmpId = employeeViewModel.EmpId;
                emp.IsMarried = (int)employeeViewModel.IsMarried == 1;
                emp.CreateOn = DateTime.Now;
                emp.HasChild = (int)employeeViewModel.HasChild == 1;
                emp.ModifiedOn = DateTime.Now;
                emp.EmployeeStatusesId = db.EmployeeStatuses.Single(x => x.Name == "Active").EmployeeStatusId;
                emp.IsDisability = (int)employeeViewModel.IsDisability == 1;
                emp.IsAboriginal = (int)employeeViewModel.IsAboriginal == 1;
                db.Employees.Add(emp);
                db.SaveChanges();

                //Education
                //var q = new List<ViewModel.Education>();
                foreach (var edu in employeeViewModel.Educations)
                {
                    var schoolname = edu.SchoolName;
                    var department = edu.Department;
                    var dgreee = edu.Drgree;
                    var sdate = edu.StartDate;
                    var enddate = edu.EndDate;

                    Models.Education edus = new Models.Education();
                    edus.EducationId = Guid.NewGuid();
                    edus.SchoolName = schoolname;
                    edus.Department = department;
                    edus.Degree = dgreee;
                    edus.StartDate = DateTime.Parse(sdate);
                    edus.EndDate = DateTime.Parse(enddate);
                    edus.EmployeeId = emp.EmployeeId;
                    db.Educations.Add(edus);
                    db.SaveChanges();
                }

                //emp.Educations.Where(x=>x.EducationId) = Guid.NewGuid();

                return RedirectToAction("Index");

            }

            return RedirectToAction("Create");
        }


        //Action result for ajax call
        [HttpPost]
        public ActionResult GetDistrictsByCityId(Guid cityId)
        {
            List<District> districts = districts = GetAllDistrict().Where(m => m.CityId.Equals(cityId)).ToList();
            SelectList districtsSelectList = new SelectList(districts, "DistrictId", "Name", null);
            return Json(districtsSelectList);
        }
        //collection for city
        public List<District> GetAllDistrict()
        {
            return db.Districts.ToList();
        }

        //public List<Models.Education> GetAllEdu()
        //{
        //    return db.Educations.ToList();
        //}


        // GET: Employees/Edit/5
        public ActionResult Edit(Guid? id,Guid? eid)
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
           

            if (employee == null)
            {
                return HttpNotFound();
            }

            //Address address = new Address();
            //address.AddressId = Guid.NewGuid();
            //address.CityId = Guid.Parse(employeeViewModel.CitySelectedValue);
            //address.DistrictId = Guid.Parse(employeeViewModel.DistrictSelectedValue);
            //address.AddressLine = employeeViewModel.AddressLine;
            //db.Addresses.Add(address);

            EmployeeViewModel empViewModel = new EmployeeViewModel();
            
            empViewModel.EmployeeId = employee.EmployeeId;
            empViewModel.PhotoPath = employee.PhotoPath;
            empViewModel.EmployeeName = employee.EmployeeName;
            empViewModel.EmployeeNameEn = employee.EmployeeNameEn;
            empViewModel.BirthDate = employee.BirthDate;
            empViewModel.Gender = employee.Gender == true ? Gender.男 : Gender.女;
            var permenentAddress = db.Addresses.Find(employee.PermanentAddressId);
            empViewModel.CitySelectedValue = permenentAddress.CityId.ToString();
            empViewModel.DistrictSelectedValue = permenentAddress.DistrictId.ToString();
            empViewModel.AddressLine = permenentAddress.AddressLine;
            var residentialAddress = db.Addresses.Find(employee.ResidentialAddressId);
            empViewModel.ResidentialCitySelectedValue = residentialAddress.CityId.ToString();
            empViewModel.ResidentialDistrictSelectedValue = residentialAddress.DistrictId.ToString();
            empViewModel.ResidentialAddressLine = residentialAddress.AddressLine;
            empViewModel.EmpId = employee.EmpId;

            //帶出原本的教育資料

            var alledu = db.Educations.Where(x => x.EmployeeId.ToString() == employee.EmployeeId.ToString()).ToList();
            List<ViewModel.Education> vedu = new List<ViewModel.Education>();
            foreach (var edus in alledu)
            {
                vedu.Add(new ViewModel.Education()
                {
                    EducationId=edus.EducationId.ToString(),
                    SchoolName = edus.SchoolName,
                    Department = edus.Department,
                    Drgree = edus.Degree,
                    StartDate = edus.StartDate.ToString(),
                    EndDate = edus.EndDate.ToString(),

                });
            }
            empViewModel.Educations =vedu;
            //foreach (var eduv in empViewModel.Educations)
            //{
            //    if (eduid.EmployeeId==employee.EmployeeId)
            //    {
            //        eduv.SchoolName = eduid.SchoolName;
            //        eduv.Department = eduid.Department;
            //        eduv.Drgree = eduid.Degree;
            //        eduv.StartDate = eduid.StartDate.ToString();
            //        eduv.EndDate = eduid.EndDate.ToString();
            //    }             

            //}


            ViewBag.City = new SelectList(db.Cities, "CityId", "Name");
            ViewBag.District = new SelectList(db.Districts, "DistrictId", "Name");
            return View(empViewModel);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeViewModel employeeViewModel,
            HttpPostedFileBase PhotoPath)
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (ModelState.IsValid)
            {
                Employee emp = db.Employees.Find(employeeViewModel.EmployeeId);
                string strPath = "";
                string newfilename = "";
                if (PhotoPath != null)
                {
                    int startindex = PhotoPath.ContentType.LastIndexOf('/') + 1;
                    string filetype = PhotoPath.ContentType.Substring(startindex); //取得副檔名
                    newfilename = Guid.NewGuid() + "." + filetype;
                    strPath = Request.PhysicalApplicationPath + @"images\Employees\" + newfilename;
                    PhotoPath.SaveAs(strPath);
                    emp.PhotoPath = newfilename;
                }

                emp.EmployeeName = employeeViewModel.EmployeeName;
                emp.Gender = (int)employeeViewModel.Gender == 1;
                emp.BirthDate = employeeViewModel.BirthDate;
                emp.EmpId = employeeViewModel.EmpId;


                // Address
                Address address = db.Addresses.Find(emp.PermanentAddressId);
                address.CityId = Guid.Parse(employeeViewModel.CitySelectedValue);
                address.DistrictId = Guid.Parse(employeeViewModel.DistrictSelectedValue);
                address.AddressLine = employeeViewModel.AddressLine;
                //Residential Address
                Address ResidentialAddress = db.Addresses.Find(emp.ResidentialAddressId);
                ResidentialAddress.CityId = Guid.Parse(employeeViewModel.ResidentialCitySelectedValue);
                ResidentialAddress.DistrictId = Guid.Parse(employeeViewModel.ResidentialDistrictSelectedValue);
                ResidentialAddress.AddressLine = employeeViewModel.ResidentialAddressLine;
                //Educations
                
                foreach (var eduEdit in employeeViewModel.Educations)
                {
                    Models.Education edus = db.Educations.Find(Guid.Parse(eduEdit.EducationId));
                    var schoolname = eduEdit.SchoolName;
                    var department = eduEdit.Department;
                    var dgreee = eduEdit.Drgree;
                    var sdate = eduEdit.StartDate;
                    var enddate = eduEdit.EndDate;

                    //Models.Education eduss = new Models.Education();
                    //edus.EducationId = Guid.NewGuid();
                    edus.SchoolName = schoolname;
                    edus.Department = department;
                    edus.Degree = dgreee;
                    edus.StartDate = DateTime.Parse(sdate);
                    edus.EndDate = DateTime.Parse(enddate);
                    edus.EmployeeId = emp.EmployeeId;
                    //db.Educations.Add(edus);
                    db.SaveChanges();
         
                }
              
                db.SaveChanges();


                //Employee
                //emp.EmployeeId = Guid.NewGuid();
                //emp.EmployeeName = employeeViewModel.EmployeeName;
                //emp.EmployeeNameEn = employeeViewModel.EmployeeNameEn;
                //emp.BirthDate = employeeViewModel.BirthDate;
                //emp.IdCardNum = employeeViewModel.IdCardNum;
                //emp.Gender = (int)employeeViewModel.Gender == 1;
                //emp.BloodType = employeeViewModel.BloodType;
                //emp.MobilePhone = employeeViewModel.MobilePhone;
                //emp.PermanentAddressId = address.AddressId;
                //emp.ResidentialAddressId = ResidentialAddress.AddressId;
                //emp.Email = employeeViewModel.Email;
                //emp.PermanentTel = employeeViewModel.PermanentTel;
                //emp.ResidentialTel = employeeViewModel.ResidentialTel;
                //emp.PhotoPath = newfilename;
                //emp.OnBoardDate = employeeViewModel.OnBoardDate;
                //emp.EmpId = employeeViewModel.EmpId;
                //emp.IsMarried = (int)employeeViewModel.IsMarried == 1;
                //emp.CreateOn = DateTime.Now;
                //emp.HasChild = (int)employeeViewModel.HasChild == 1;
                //emp.ModifiedOn = DateTime.Now;
                //emp.EmployeeStatusesId = db.EmployeeStatuses.Single(x => x.Name == "Active").EmployeeStatusId;
                //emp.IsDisability = (int)employeeViewModel.IsDisability == 1;
                //emp.IsAboriginal = (int)employeeViewModel.IsAboriginal == 1;
                //db.Employees.Add(emp);
                //db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(employeeViewModel);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            //Models.Education education = db.Educations.Find(educations);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id, List<Guid> ids)
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            foreach (var eduid in ids)
            {
                Models.Education edu = db.Educations.Find(eduid);
                db.Educations.Remove(edu);
                db.SaveChanges();
            }

            //foreach (var eduEdit in employeeViewModel.Educations)
            //{
            //    Models.Education edus = db.Educations.Find(Guid.Parse(eduEdit.EducationId));
            //    var schoolname = eduEdit.SchoolName;
            //    var department = eduEdit.Department;
            //    var dgreee = eduEdit.Drgree;
            //    var sdate = eduEdit.StartDate;
            //    var enddate = eduEdit.EndDate;

            //    //Models.Education eduss = new Models.Education();
            //    //edus.EducationId = Guid.NewGuid();
            //    //edus.SchoolName = schoolname;
            //    //edus.Department = department;
            //    //edus.Degree = dgreee;
            //    //edus.StartDate = DateTime.Parse(sdate);
            //    //edus.EndDate = DateTime.Parse(enddate);
            //    //edus.EmployeeId = employee.EmployeeId;
            //    //db.Educations.Add(edus);
            //    db.Educations.Remove(edus);
            //    db.SaveChanges();

            //}
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Deletes")]
        public ActionResult DeletesConfirmed(List<Guid> ids)
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            foreach (var item in ids)
            {
                Employee emp = db.Employees.Find(item);
                db.Employees.Remove(emp);
                db.SaveChanges();


            }
            return RedirectToAction("Index");
        }




        [HttpPost]
        public ActionResult Export()
        {
            var exportSpource = this.GetExportData();
            var dt = JsonConvert.DeserializeObject<DataTable>(exportSpource.ToString());

            var exportFileName = string.Concat(
                "EmployeeIndex",
                DateTime.Now.ToString("yyyyMMddHHmmss"),
                ".xlsx");

            return new ExportExcelResult
            {
                SheetName = "EmployeeIndex",
                FileName = exportFileName,
                ExportData = dt
            };
        }

        private JArray GetExportData()
        {
            var query = db.Employees;

            JArray jObjects = new JArray();

            foreach (var item in query)
            {
                var jo = new JObject();
                jo.Add("GID", item.EmpId);
                jo.Add("姓名", item.EmployeeName + item.EmployeeNameEn);
                jo.Add("性別", item.Gender);
                jo.Add("身分證字號", item.IdCardNum);
                jo.Add("地址", item.PermanentAddressId);
                //jo.Add("Town", item.Town);
                //jo.Add("Sequence", item.Sequence);
                jObjects.Add(jo);
            }
            return jObjects;
        }
        protected override void Dispose(bool disposing)
        {

            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
