﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
            var employees = db.Employees.Include(e => e.PermanentAddress).Include(e => e.ResidentialAddress).Include(e => e.EmployeeStatus).Include(e => e.EmployeeStatus1);
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
            ViewBag.PermanentAddressId = new SelectList(db.PermanentAddresses, "PermanentAddressId", "AddressLine");
            ViewBag.ResidentialAddressId = new SelectList(db.ResidentialAddresses, "ResidentialAddressId", "AddressLine");
            ViewBag.EmployeeId = new SelectList(db.EmployeeStatuses, "EmployeeStatusId", "Name");
            ViewBag.EmployeeStatusesId = new SelectList(db.EmployeeStatuses, "EmployeeStatusId", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeId,EmployeeName,EmployeeNameEn,BirthDate,IdCardNum,Gender,BloodType,MobilePhone,Email,PermanentAddressId,ResidentialAddressId,PermanentTel,ResidentialTel,Photo,OnBoardDate,EmpId,IsMarried,CreateOn,HasChild,NumberOfChild,ModifiedOn,EmployeeStatusesId,IsDisability,IsAboriginal")] EmployeeViewModel employeeViewModel)
        {
            if (ModelState.IsValid)
            {
                Employee emp = new Employee();
                emp.EmployeeId = Guid.NewGuid();
                emp.EmployeeName = employeeViewModel.EmployeeName;
                emp.EmployeeNameEn = employeeViewModel.EmployeeNameEn;
                emp.BirthDate = employeeViewModel.BirthDate;
                emp.IdCardNum = employeeViewModel.IdCardNum;
                emp.Gender = (int)employeeViewModel.Gender == 1;
                emp.BloodType = employeeViewModel.BloodType;
                emp.MobilePhone = employeeViewModel.MobilePhone;
                emp.Email = employeeViewModel.Email;
                emp.PermanentAddressId = employeeViewModel.PermanentAddressId;
                emp.ResidentialAddressId = employeeViewModel.ResidentialAddressId;
                emp.PermanentTel = employeeViewModel.PermanentTel;
                emp.ResidentialTel = employeeViewModel.ResidentialTel;
                emp.Photo = employeeViewModel.Photo;
                emp.OnBoardDate = employeeViewModel.OnBoardDate;
                emp.EmpId = employeeViewModel.EmpId;
                emp.IsMarried = (int)employeeViewModel.IsMarried==1;
                emp.CreateOn = DateTime.Now;
                emp.HasChild = (int)employeeViewModel.HasChild==1;
                emp.ModifiedOn = DateTime.Now;
                emp.EmployeeStatusesId = db.EmployeeStatuses.Single(x => x.Name == "Active").EmployeeStatusId;
                emp.IsDisability = (int)employeeViewModel.IsDisability==1;
                emp.IsAboriginal = (int)employeeViewModel.IsAboriginal==1;
                db.Employees.Add(emp);
                db.SaveChanges();
                return RedirectToAction("Index");
               
            }


            return View(employeeViewModel);
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
            ViewBag.PermanentAddressId = new SelectList(db.PermanentAddresses, "PermanentAddressId", "AddressLine", employee.PermanentAddressId);
            ViewBag.ResidentialAddressId = new SelectList(db.ResidentialAddresses, "ResidentialAddressId", "AddressLine", employee.ResidentialAddressId);
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
            ViewBag.PermanentAddressId = new SelectList(db.PermanentAddresses, "PermanentAddressId", "AddressLine", employee.PermanentAddressId);
            ViewBag.ResidentialAddressId = new SelectList(db.ResidentialAddresses, "ResidentialAddressId", "AddressLine", employee.ResidentialAddressId);
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