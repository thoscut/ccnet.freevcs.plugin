Plugin Jedi VCS pour Cruise Control .Net

I	Pr�sentation
II	Installation
III	Configuration
IV 	Historique

I	Pr�sentation

Le plugin Jedi VCS (http:/jedivcs.sourceforge.net) pour Cruise Control .Net (http://ccnet.sourceforge.net) permet � ce dernier d'utiliser le premier.
Cruise Control .Net est un outil d'int�gration continue et Jedi VCS est un syst�me de gestion de version.

II	Installation

Vous devez copier le fichier ccnet.jedivcs.plugin.dll dans le r�pertoire server ou est install� Cruise Control .Net.
Ensuite vous red�marrez l'application console ou bien le service de CCNET.

III Configuration

La configuration du fichier << ccnet.config >> devra au minumum  comporter les �l�ments ci-dessous :

<?xml version="1.0"?>
<cruisecontrol>
 <project name="Test">
	<sourcecontrol type="jedivcs">
	 <server>Zeus</server>
	 <userName>CCNET</userName>
	 <password>ccnet</password>
	 <jedivcspath>D:\Program Files\JEDI\JVCS</jedivcspath>
	 <projectName>Test</projectName>
	</sourcecontrol>
 </project>
</cruisecontrol>

Vous pouvez �galement ajouter les �l�ments suivants :

<serverPort> "port" </serverPort> : le port du serveur, dans le cas ou celui-ci serait diff�rent du port par d�faut ;

<projectId> " identifiant du projet "</projectId> : l'identifiant unique du projet ;

<projectType> " type de projet " </projectType> : Le type de projet ;

<labelOnSuccess> " labellisation si succ�s de la construction " </labelOnSuccess>

<branch> " Branche du projet " </branch>

<methodGetSource> " M�thode d'obtention des sources " </methodGetSource> : synchronize ou getmodule

IV	Historique

Le plugind Jedi VCS pour Cruise Control .Net est pour l'instant en version Beta 1.
Vous pouvez faire les bugs ou vos remarques au d�veloppeur : frederic.libaud@club-internet.fr