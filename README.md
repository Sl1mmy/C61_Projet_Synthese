# Projet Synthèse
 
Rapport pour le Sprint 3 du Projet Synthèse.

## État courant du projet

À la fin du sprint 3, nous avons réussi à implémenter la méthode de rendu Raymarching, ainsi que la modification de celle-ci afin d'implémenter nos objets 4D, nous avons aussi implémenter un prefab `Objet4D` générique ainsi que des opérateurs booléens tels que *union* et *substract*, ce qui nous permet de créer des formes plus complexes avec les formes de bases que l'on peux créer (ex: un arbre créé avec un cylindre et plusieurs sphères). En dehors de l'implémentation 4D, nous avons créé l'interface de jeu, les menus et le menu option (sans fonctionnalité), la sélection de *save file*, la sélection de niveaux ainsi que certains niveaux eux même (sans 4D). Les prefabs pour le joueur et l'objet de but (représenté par une fusée) sont aussi fait.


## Ce qu'il reste à faire

Premièrement, nous devons implémenter la dimension 4D dans nos niveaux. Nous ferons cela en ajoutant la caméra de raymarching 4D dans nos niveaux et nous ajusterons les niveaux en conscéquence.

Ensuite, nous devons nous concentrer sur le design des niveaux et de leurs mécaniques. C'est a dire ajuster les niveaux selon les méchaniques 4D afin de créer des défis intéressant et unique à la 4D.

Une fois les niveaux conçus, nous devons également rendre le jeu visuellement attrayant en utilisant le shader de pixelisation.

Ensuite, nous devons ajouter les fonctionnalités aux options du jeu. Cela inclut divers paramètres et réglages qui permettront aux joueurs de personnaliser leur expérience de jeu selon leurs préférences.

Enfin, l'intégration de la base de données. Cela implique la mise en place d'un système de gestion des données efficace pour suivre la progression des joueurs, gérer les scores et autres informations pertinentes.