using System;

namespace IdentityService.Domain
{
    public    class ResetTicket
    {
        public DateTime? ExpireDate { get; set; }
        public Guid? Ticket { get; set; }
    }
}
