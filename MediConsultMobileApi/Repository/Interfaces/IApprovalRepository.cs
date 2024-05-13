﻿using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IApprovalRepository
    {
        Task<List<Approval>> GetAll(int memberId);
        Approval GetByApprovalId(int approvalId);
        void AddApproval(int memberId, Approval approval);
        void Canceled(int approvalId);
        bool ApprovalExists(int approvalId);
        void Save();
    }
}
