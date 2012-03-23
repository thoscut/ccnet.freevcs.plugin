Plugin Jedi VCS pour Cruise Control .Net

I	Présentation
II	Installation
III	Configuration
IV 	Historique

I	Présentation

Le plugin Jedi VCS (http:/jedivcs.sourceforge.net) pour Cruise Control .Net (http://ccnet.sourceforge.net) permet à ce dernier d'utiliser le premier.
Cruise Control .Net est un outil d'intégration continue et Jedi VCS est un système de gestion de version.

II	Installation

Vous devez copier le fichier ccnet.jedivcs.plugin.dll dans le répertoire server ou est installé Cruise Control .Net.
Ensuite vous redémarrez l'application console ou bien le service de CCNET.

III Configuration

La configuration du fichier << ccnet.config >> devra au minumum  comporter les éléments ci-dessous :

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

Vous pouvez également ajouter les éléments suivants :

<serverPort> "port" </serverPort> : le port du serveur, dans le cas ou celui-ci serait différent du port par défaut ;

<projectId> " identifiant du projet "</projectId> : l'identifiant unique du projet ;

<projectType> " type de projet " </projectType> : Le type de projet ;

<labelOnSuccess> " labellisation si succès de la construction " </labelOnSuccess>

<branch> " Branche du projet " </branch>

<methodGetSource> " Méthode d'obtention des sources " </methodGetSource> : synchronize ou getmodule

IV	Historique

Le plugind Jedi VCS pour Cruise Control .Net est pour l'instant en version Beta 1.
Vous pouvez faire les bugs ou vos remarques au développeur : frederic.libaud@club-internet.fr