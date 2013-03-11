<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="AzureCodeCamp.Azure" generation="1" functional="0" release="0" Id="5c53bad0-f179-46b3-a6a6-394c15e2e7eb" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="AzureCodeCamp.AzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="AzureCodeCamp:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/AzureCodeCamp.Azure/AzureCodeCamp.AzureGroup/LB:AzureCodeCamp:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="AzureCodeCamp:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AzureCodeCamp.Azure/AzureCodeCamp.AzureGroup/MapAzureCodeCamp:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="AzureCodeCamp:Setting1" defaultValue="">
          <maps>
            <mapMoniker name="/AzureCodeCamp.Azure/AzureCodeCamp.AzureGroup/MapAzureCodeCamp:Setting1" />
          </maps>
        </aCS>
        <aCS name="AzureCodeCampInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AzureCodeCamp.Azure/AzureCodeCamp.AzureGroup/MapAzureCodeCampInstances" />
          </maps>
        </aCS>
        <aCS name="WorkerRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AzureCodeCamp.Azure/AzureCodeCamp.AzureGroup/MapWorkerRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="WorkerRole1Instances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AzureCodeCamp.Azure/AzureCodeCamp.AzureGroup/MapWorkerRole1Instances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:AzureCodeCamp:Endpoint1">
          <toPorts>
            <inPortMoniker name="/AzureCodeCamp.Azure/AzureCodeCamp.AzureGroup/AzureCodeCamp/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapAzureCodeCamp:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureCodeCamp.Azure/AzureCodeCamp.AzureGroup/AzureCodeCamp/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapAzureCodeCamp:Setting1" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureCodeCamp.Azure/AzureCodeCamp.AzureGroup/AzureCodeCamp/Setting1" />
          </setting>
        </map>
        <map name="MapAzureCodeCampInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AzureCodeCamp.Azure/AzureCodeCamp.AzureGroup/AzureCodeCampInstances" />
          </setting>
        </map>
        <map name="MapWorkerRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureCodeCamp.Azure/AzureCodeCamp.AzureGroup/WorkerRole1/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapWorkerRole1Instances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AzureCodeCamp.Azure/AzureCodeCamp.AzureGroup/WorkerRole1Instances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="AzureCodeCamp" generation="1" functional="0" release="0" software="C:\Users\Erkki\documents\visual studio 2012\Projects\AzureCodeCamp\AzureCodeCamp.Azure\csx\Debug\roles\AzureCodeCamp" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="Setting1" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;AzureCodeCamp&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;AzureCodeCamp&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;WorkerRole1&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AzureCodeCamp.Azure/AzureCodeCamp.AzureGroup/AzureCodeCampInstances" />
            <sCSPolicyUpdateDomainMoniker name="/AzureCodeCamp.Azure/AzureCodeCamp.AzureGroup/AzureCodeCampUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/AzureCodeCamp.Azure/AzureCodeCamp.AzureGroup/AzureCodeCampFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="WorkerRole1" generation="1" functional="0" release="0" software="C:\Users\Erkki\documents\visual studio 2012\Projects\AzureCodeCamp\AzureCodeCamp.Azure\csx\Debug\roles\WorkerRole1" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WorkerRole1&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;AzureCodeCamp&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;WorkerRole1&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AzureCodeCamp.Azure/AzureCodeCamp.AzureGroup/WorkerRole1Instances" />
            <sCSPolicyUpdateDomainMoniker name="/AzureCodeCamp.Azure/AzureCodeCamp.AzureGroup/WorkerRole1UpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/AzureCodeCamp.Azure/AzureCodeCamp.AzureGroup/WorkerRole1FaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="AzureCodeCampUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="WorkerRole1UpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="AzureCodeCampFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="WorkerRole1FaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="AzureCodeCampInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="WorkerRole1Instances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="30329fa5-e157-42d7-9267-fc0886977f3d" ref="Microsoft.RedDog.Contract\ServiceContract\AzureCodeCamp.AzureContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="bbd93d88-ae4b-4100-bce2-199f1fca32ff" ref="Microsoft.RedDog.Contract\Interface\AzureCodeCamp:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/AzureCodeCamp.Azure/AzureCodeCamp.AzureGroup/AzureCodeCamp:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>