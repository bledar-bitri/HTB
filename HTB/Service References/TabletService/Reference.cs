﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HTB.TabletService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="TabletService.IGetData")]
    public interface IGetData {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGetData/GetPhoneTypes", ReplyAction="http://tempuri.org/IGetData/GetPhoneTypesResponse")]
        string GetPhoneTypes();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGetData/GetAktTypes", ReplyAction="http://tempuri.org/IGetData/GetAktTypesResponse")]
        string GetAktTypes();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGetDataChannel : HTB.TabletService.IGetData, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetDataClient : System.ServiceModel.ClientBase<HTB.TabletService.IGetData>, HTB.TabletService.IGetData {
        
        public GetDataClient() {
        }
        
        public GetDataClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public GetDataClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GetDataClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GetDataClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetPhoneTypes() {
            return base.Channel.GetPhoneTypes();
        }
        
        public string GetAktTypes() {
            return base.Channel.GetAktTypes();
        }
    }
}
