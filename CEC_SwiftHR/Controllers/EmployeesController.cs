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
using CEC_SwiftHR.ViewModel;

namespace CEC_SwiftHR.Controllers
{
    public class EmployeesController : Controller
    {
        private NewEmployeeEntities db = new NewEmployeeEntities();

        // GET: Employees
        public ActionResult Index()
        {
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
            if (ModelState.IsValid)
            {
                string strPath = "";

                if (PhotoPath != null)
                {
                    int startindex = PhotoPath.ContentType.LastIndexOf('/') + 1;
                    string filetype = PhotoPath.ContentType.Substring(startindex); //取得副檔名
                    string newfilename = Guid.NewGuid() + "." + filetype;
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
                emp.Email = employeeViewModel.Email;
                emp.PermanentTel = employeeViewModel.PermanentTel;
                emp.ResidentialTel = employeeViewModel.ResidentialTel;
                emp.PhotoPath = strPath;
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
                return RedirectToAction("Index");

            }

            return View(employeeViewModel);
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



























        // GET: Employees/Edit/5
        public ActionResult Edit(Guid? id)
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
            //ViewBag.PermanentAddressId = new SelectList(db.PermanentAddresses, "PermanentAddressId", "AddressLine", employee.PermanentAddressId);
            //ViewBag.ResidentialAddressId = new SelectList(db.ResidentialAddresses, "ResidentialAddressId", "AddressLine", employee.ResidentialAddressId);
            ViewBag.EmployeeId = new SelectList(db.EmployeeStatuses, "EmployeeStatusId", "Name", employee.EmployeeId);
            ViewBag.EmployeeStatusesId = new SelectList(db.EmployeeStatuses, "EmployeeStatusId", "Name", employee.EmployeeStatusesId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeId,EmployeeName,EmployeeNameEn,BirthDate,IdCardNum,Gender,BloodType,MobilePhone,Email,PermanentAddressId,ResidentialAddressId,PermanentTel,ResidentialTel,Photo,OnBoardDate,EmpId,IsMarried,CreateOn,HasChild,NumberOfChild,ModifiedOn,EmployeeStatusesId,IsDisability,IsAboriginal")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.PermanentAddressId = new SelectList(db.PermanentAddresses, "PermanentAddressId", "AddressLine", employee.PermanentAddressId);
            //ViewBag.ResidentialAddressId = new SelectList(db.ResidentialAddresses, "ResidentialAddressId", "AddressLine", employee.ResidentialAddressId);
            ViewBag.EmployeeId = new SelectList(db.EmployeeStatuses, "EmployeeStatusId", "Name", employee.EmployeeId);
            ViewBag.EmployeeStatusesId = new SelectList(db.EmployeeStatuses, "EmployeeStatusId", "Name", employee.EmployeeStatusesId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(Guid? id)
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

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
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
