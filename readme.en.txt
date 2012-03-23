Plugin Jedi VCS for Cruise Control .Net 

I 	Présentation 
II 	Installation 
III Configuration 
IV 	Historique 

I Présentation

The plugin Jedi VCS ( http://jedivcs.sourceforge.net ) for Cruise Control .Net (http://ccnet.sourceforge.net) allows this last one to use the first one.
Cruise Control .Net is a tool of continuous integration and Jedi VCS is a management system of version.

II	Installation

You have to copy the ccnet.jedivcs.plugin.dll file in the directory server or is installed Cruise Control .Net.
Then you restart the application console either the service of CCNET.

III Configuration

The configuration of the file < < ccnet.config > > will have to to the minumum contain elements below:

<?xml version="1.0"?>
<cruisecontrol>
 <project name="MyProject">
	<sourcecontrol type="jedivcs">
	 <server>My Jedi VCS Server name or ip address</server>
	 <userName>The CCNET account for Jedi VCS</userName>
	 <password>The password for CCNET account</password>
	 <jedivcspath>C:\Program Files\JEDI\JVCS</jedivcspath>
	 <projectName>The project name</projectName>
	</sourcecontrol>
 </project>
</cruisecontrol>

You can also add the following elements:

<serverPort> "port server" </serverPort> : The port of the server, in the case or this one would be different from the port(bearing) by default ;

<projectId> " identifiant du projet "</projectId> : The unique identifier of the project;

<projectType> " project type " </projectType> : The project type ;

<labelOnSuccess> " Labellisation if success of the construction " </labelOnSuccess> : boolean value (true or false) ;

<branch> " The project branch " </branch> : Project branch ;

<methodGetSource> " Method to obtain update of source " </methodGetSource> : synchronize ou getmodule

IV	Historique

The plugind Jedi VCS for Cruise Control .Net is at the moment in version Beta 1.
You can make bugs or your remarks for the developer: frederic.libaud@club-internet.