
Pyramiden Tools:

Sequence Editor (My Tools/Sequence Editor):

Le Sequence Editor est le coeur de l’outil. il va vous permettre de créer et de modifier une suite d’événements (ici appelées “nodes“) tel qu’afficher un texte, une image ou attendre un clic.
En ouvrant le Sequence Editor, vous allez arrivez sur une fenêtre comprenant toutes les sequences créés jusqu’à maintenant. Pour chaque sequence, vous pouvez modifier son nom (l’identifiant qui vous permettra de faire référence à cette sequence) et sa couleur (inutilisé pour l’instant). En cliquant sur “Edit“, vous allez ouvrir une nouvelle fenêtre contenant un éditeur nodal.
Pour rajouter une node, il suffit de cliquer droit sur la fenêtre et de sélectionner la node voulue. L’éditeur va commencer à la node “isFirst“ et continuer en suivant les flèches.
Pour ajouter une flèches entre deux nodes, cliquez droit sur une node, sélectionnez “Make Transition“ et cliquez gauche sur la node vers laquelle vous voulez que la flèche aille (notez que pour certaines nodes comme les choix, plusieurs “Make Transition“ seront possibles).
Pour faire d’une node la première node de la sequence, cliquez droit sur une node et sélectionnez “Set First“.
Vous pouvez également supprimer à tout moment une node en cliquant droit dessus et en sélectionnant “Delete Node“.
Vous avez également la possibilité de vous déplacer dans l’éditeur en maintenant un clic sur la fenêtre tout en bougeant votre souris. Vous pouvez aussi bouger les nodes avec un drag and drop.
Voici la liste des nodes actuellement disponibles (les options présentes sont les mêmes que dans Emma):

Text Node: Affiche une phrase. par défaut la phrase s’écrira entièrement puis le programme passera à la node suivante. Vous pouvez également ajouter une voix qui sera jouée par dessus le texte.
Image Node: Affiche une image. Vous pouvez modifier sa position, rotation et taille.
GameObject Node: Instantie un prefab. même modifications que pour l’image (Le prefab doit appartenir au layer “GameObject“).
Choice Node: Affiche un choix. Vous pouvez ajouter jusqu’à 4 choix différents, chacun aura une “Make transition“ portant son numéro.
Click Node: Attends un clic de la part du joueur avant de passer à la node suivante (à mettre après chaque ligne de texte si vous voulez une déroulant semblable à celui d’Emma).

Lucidity Node: Permet d’ajouter ou d’enlever de la lucidité au personnage.
Calcul Node: Permet d’affecter ou d’incrémenter une variable personnalisée. Choisissez juste son nom et sa valeur.
Condition Node: Permet d’ajouter une condition en fonction des variables modifiées. Sélectionnez le type de comparaison puis entré le nom de votre variable et à quoi vous voulez la comparer (pour tester la lucidité, entrez “Lucidity“). Avec cette node vous pouvez faire deux transitions, une si la condition est vraie(true) et une si elle est fausse(false).

Wait Node: Permet d’ajouter une pause en secondes. Le programme attendra la durée entrée avant de passer à la node suivante.
Fade Node: Permet d’ajouter un Fade In ou un Fade Out. L’écran prendra la couleur donnée en passant sur la node Fade Out et restera ainsi jusqu’à la node Fade In.
Animation Node: Permet de lancer une animation sur une image ou un gameObject (décochez la case “Image“ si vous voulez animer un gameObject). Sélectionnez d’abord l’image/gameObject à animer puis choisissez la durée de l’animation, ainsi que sa position, rotation et taille d’arrivée (sa position, rotation et taille de départ seront celles de l’image/gameObject au moment d’arriver sur la node).
Destroy Node: permet de détruire une image ou un gameObject (Sélectionnez l’image/gameObject de la même façon que l’Animation Node).

Sequence Node: Permet de lancer une autre sequence. Entrez le nom de la sequence à lancer. Une fois la séquence finie, le programme passera à la node suivante.
Place Node: Permet de charger un nouveau lieu. Entrez juste le nom du lieu à charger.
Exploration Node: Permet de lancer une phase de point & click. Entrez juste le nom de la phase à charger.

Play Sound Node: Permet de lancer un son ou une musique. Vous pouvez modifier sa durée d’apparition, son volume et si le son boucle. Si le son ne boucle pas, il sera automatiquement détruit lorsqu’il se finira.
Stop Sound Node: Permet d’arrêter un son ou une musique. Cochez Stop All si vous voulez arrêter toute les musiques et son qui tournent actuellement. Sinon, sélectionnez la musique à couper. Vous pouvez choisir la durée de disparition de la musique.

Places Editor (My Tools/Places Editor):

Le Places Editor va permettre d’ajouter et de modifier des lieux.
Comme pour les séquences, ajouter un lieu avec “New“ et modifiez son nom puis cliquez sur “Edit“ pour le modifier.
Vous allez alors pouvoir modifier la représentation du lieu sur la carte (pas encore utilisé), le niveau minimal de lucidité nécéssaire et la première séquence qui se lancera en arrivant dans ce lieu.
Au début du jeu, le premier lieu de la liste sera chargé.

Character Editor (My Tools/Character Manager):

Le Character Editor fonctionne exactement comme celui de Emma.
Assurez vous bien d’avoir toujours au moins un personnage avant de rajouter des textes dans l’éditeur de sequence.

Exploration Editor (My Tools/Exploration Editor):

L’Exploration Editor va vous permettre d’ajouter et de modifier des phases de point & click.
Cet éditeur fonctionne différemment des autres. cliquez sur “New“ va créer une nouvelle scène, qui correspondra à une nouvelle phase de point & click.
Cliquez ensuite sur “Edit“ pour modifier la scène. commencer alors par drag & drop une image dans la zone Background. cela vous donnera un fond de base pour votre phase. cliquez sur le bouton “Edit“ situé sous ce cadre pour sélectionner l’image dans la scène et la redimensionner à votre convenance.
Comme pour le lieu, vous pouvez ajouter une sequence qui se jouera au chargement de cette phase en entrant son nom dans le champ “Begining sequence“ mais contrairement à un lieu cette sequence est optionnel et une fois finie, elle laissera le joueur sur la phase de point & click.
Cliquez sur “New“ pour ajouter une zone d’interaction. Entrez alors le nom de la sequence à charger quand le joueur cliquera sur cette zone et cliquez sur “Edit“ pour sélectionner la zone et modifier sa taille et sa position.
Comme pour tout les autres éditeurs, n’oubliez pas de cliquer sur “Save“ dans la fenêtre de détail pour sauvegarder vos changements.
Fermer l’Exploration Editor vous ramènera sur la scène sur laquelle vous étiez avant de le lancer.

Autres:

La scène Main est la scène par défaut du jeu. Lancez toujours le jeu à partir de cette scène.

En jeu, vous pouvez ouvrir la “Map“ en cliquant sur le bouton en haut à droite de l’écran. Il vous proposera une liste de tout les lieux disponibles avec votre niveau de lucidité et vous pourrez lancer allez sur ces lieu en cliquant sur le nom du lieu voulu.






