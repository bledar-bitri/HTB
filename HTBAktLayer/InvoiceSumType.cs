using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTBAktLayer
{
    public enum InvoiceSumType
    {
        OpenedAll,
        OpenedAmountOfOriginalInvoice,
        OpenedAmountOfCollectionInvoices,
        CollectionInvoice,
        OriginalInvoice,
        ClientCost,
        OpenedAmountOfClientCost,
        Interest,
        Payment
    };
}
