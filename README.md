# TP03 – Unity 3D

Projet réalisé dans le cadre du cours **Interface Development and Design**.

L’objectif de ce TP est de construire progressivement une scène interactive dans Unity en combinant le déplacement d’un joueur, une caméra orbitale, des animations, un système de combat, des courbes de Bézier et une visite guidée d’une forêt.

## Fonctionnalités réalisées

### Exercice 1 – Déplacement du joueur

- Déplacement avec **Z/Q/S/D** ou **W/A/S/D**.
- Saut avec la touche **Espace** uniquement lorsque le joueur touche le sol.
- Accélération et décélération progressives pour rendre le mouvement plus fluide.
- Déplacement basé sur un `Rigidbody`.

### Exercice 2 – Caméra orbitale

- Suivi du joueur à distance constante.
- Rotation de la caméra avec la souris.
- **Clic gauche** : rotation de la caméra uniquement.
- **Clic droit** : rotation de la caméra et du joueur dans la même direction.
- Zoom limité avec la molette.
- Limitation de l’angle vertical.

### Exercice 3 – Cycle jour/nuit

- Rotation automatique de la `Directional Light`.
- Variation de l’intensité lumineuse entre le jour et la nuit.
- Simulation d’une journée complète en environ deux minutes.
- Répétition automatique du cycle.

### Exercice 4 – Personnage animé

- Intégration d’un personnage 3D provenant de **Starter Assets**.
- Animations reliées aux déplacements et au saut.
- Gestion des événements de pas et d’atterrissage avec `AnimationEventReceiver.cs`.

### Exercice 5 – Monstre et système de combat

- Intégration d’un zombie provenant du pack **Stylised Zombie** de GreyratsLab.
- États d’attente et d’attaque gérés avec un `Animator`.
- Zone de détection créée avec un `Sphere Collider` configuré en trigger.
- Attaque automatique lorsque le joueur entre dans la zone du monstre.
- Points de vie et dégâts distincts pour le joueur et le monstre.
- Barres de vie qui suivent les personnages et s’actualisent en temps réel.

| Personnage | Points de vie | Dégâts |
|---|---:|---:|
| Joueur | 100 | 20 |
| Monstre | 100 | 10 |

### Exercice 6 – Courbes de Bézier

- Courbe quadratique avec trois points de contrôle.
- Courbe cubique avec quatre points de contrôle.
- Affichage de la trajectoire avec un `LineRenderer`.
- Mise à jour en temps réel lorsque les points sont déplacés dans l’éditeur.
- Choix entre le mode quadratique et le mode cubique depuis l’Inspector.

### Exercice 7 – Visite de la forêt

- Génération d’un terrain avec une texture procédurale.
- Ajout d’arbres et d’herbe en 3D à partir du pack **Low Poly Trees and Vegetation** de HQP Studios.
- Répartition aléatoire des éléments avec des variations de rotation et d’échelle.
- Déplacement de la caméra le long d’une courbe de Bézier cubique.
- Vitesse rendue approximativement constante grâce à une table de longueurs cumulées.
- Retour progressif vers le plan principal à la fin de la visite.
- Réactivation de la caméra orbitale après le retour.

Une vidéo de démonstration est disponible dans le dossier [`Video`](Video/).

## Commandes

| Commande | Action |
|---|---|
| `Z` / `Q` / `S` / `D` ou `W` / `A` / `S` / `D` | Déplacer le joueur |
| `Espace` | Sauter lorsque le joueur est au sol |
| Clic gauche + souris | Faire tourner uniquement la caméra |
| Clic droit + souris | Faire tourner la caméra et le joueur |
| Molette | Rapprocher ou éloigner la caméra |

## Scripts principaux

| Script | Rôle |
|---|---|
| `PlayerMovement.cs` | Déplacement, saut et animations du joueur |
| `OrbitCamera.cs` | Suivi, rotation et zoom de la caméra |
| `DayNightCycle.cs` | Cycle jour/nuit |
| `AnimationEventReceiver.cs` | Réception des événements d’animation |
| `CharacterStats.cs` | Points de vie et dégâts |
| `MonsterAttack.cs` | Détection du joueur et attaque du monstre |
| `WorldHealthBar.cs` | Affichage et mise à jour des barres de vie |
| `BezierCurve.cs` | Calcul et affichage des courbes de Bézier |
| `ForestTour.cs` | Génération de la forêt et visite guidée |

## Assets utilisés

- **First Person + Third Person Character Controllers – Starter Assets**, Unity Technologies, version 2.0.1.
- **Stylised Zombie**, GreyratsLab.
- **Low Poly Trees and Vegetation – Pack**, HQP Studios.

## Ouvrir le projet

1. Cloner le dépôt :

   ```bash
   git clone https://github.com/myriamHAJI/TP03-Unity3D.git
   ```

2. Ouvrir **Unity Hub**.
3. Cliquer sur **Add** ou **Open**.
4. Sélectionner le dossier cloné.
5. Ouvrir la scène principale.
6. Lancer la scène avec le bouton **Play**.

Le projet a été réalisé avec **Unity 6.5 (6000.5.10f1)**.

## Difficultés rencontrées

Le premier personnage testé avec Genies Avatar SDK provoquait des erreurs de compilation dans Unity 6.5 à cause d’API devenues obsolètes. Le package a été retiré et remplacé par Starter Assets.

Pour le monstre, les transitions de l’Animator ont été ajustées afin que l’attaque commence immédiatement lorsque le joueur entre dans la zone, puis se termine avant le retour à l’état d’attente.

Lors de la création de la forêt, le terrain apparaissait sans arbres ni herbe. Les prefabs avaient été assignés dans les références par défaut du fichier script au lieu du composant `Forest Tour` présent sur la `Main Camera`. Leur réassignation dans ce composant a corrigé le problème.

Enfin, la caméra s’arrêtait initialement au dernier point de la courbe. Une phase de retour a été ajoutée pour retrouver progressivement le cadrage principal, réactiver la caméra orbitale et terminer correctement la visite.

## Limites

Les deux exercices bonus du sujet n’ont pas été réalisés :

- gestion des obstacles entre la caméra et le joueur avec un Raycast ;
- courbe de Bézier récursive avec un nombre quelconque de points.

Les barres de vie sont créées par script en espace monde au lieu d’utiliser un Canvas. Elles suivent néanmoins les personnages, restent orientées vers la caméra et reflètent leurs points de vie en temps réel.

## Auteure

**Myriam HAJI**
