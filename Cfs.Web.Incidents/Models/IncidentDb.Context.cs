﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cfs.Web.Incidents.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CfsMasterEntities : DbContext
    {
        public CfsMasterEntities()
            : base("name=CfsMasterEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<IncidentAttachment> IncidentAttachments { get; set; }
        public virtual DbSet<IncidentCategory> IncidentCategories { get; set; }
        public virtual DbSet<IncidentProgram> IncidentPrograms { get; set; }
        public virtual DbSet<IncidentStaff> IncidentStaffs { get; set; }
        public virtual DbSet<IncidentType> IncidentTypes { get; set; }
        public virtual DbSet<ReportIncident> ReportIncidents { get; set; }
        public virtual DbSet<ReportType> ReportTypes { get; set; }
        public virtual DbSet<CostCenter> CostCenters { get; set; }
        public virtual DbSet<IncidentReport> IncidentReports { get; set; }
        public virtual DbSet<ReportStatus> ReportStatuses { get; set; }
        public virtual DbSet<IncidentDetail> IncidentDetails { get; set; }
        public virtual DbSet<NotifyParty> NotifyParties { get; set; }
        public virtual DbSet<ReportSign> ReportSigns { get; set; }
        public virtual DbSet<Restraint> Restraints { get; set; }
        public virtual DbSet<ApprovalStatus> ApprovalStatuses { get; set; }
        public virtual DbSet<ReportSigType> ReportSigTypes { get; set; }
        public virtual DbSet<ReportLog> ReportLogs { get; set; }
        public virtual DbSet<Medical> Medicals { get; set; }
        public virtual DbSet<AdminComment> AdminComments { get; set; }
        public virtual DbSet<ProgramNotifier> ProgramNotifiers { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
    }
}
