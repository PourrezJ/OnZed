AddEvent("OnPackageStart", function()
	print("OnPackageStart")
	print(GetWorld():GetMapName())
	
      -- If the current map is not the Office2 map then load it.
	if GetWorld():GetMapName() ~= "Office2" then
		LoadPak("OfficeMap", "/OfficeMap/", "../../../OnsetModding/Plugins/OfficeMap/Content/")
	
		local mapname = "/OfficeMap/Office/Maps/Office2"
		ConnectToServer(GetServerIP(), GetServerPort(), "", mapname)
	end
end)

AddEvent("ClientConnected", function()
    print("ClientConnected")

    
end)

AddEvent("LaunchCharCreator", function()
    print("LaunchCharCreator")

    
end)

AddEvent("OnNPCStreamIn", function(npcid)
	print("OnNPCStreamIn")
end)

AddEvent("OnNPCStreamOut", function(npcid)
	print("OnNPCStreamOut")
end)
