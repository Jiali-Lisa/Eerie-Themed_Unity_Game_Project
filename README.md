# Project 2 Report

### Role and Responsibility
* My Role: Project Manager & Environment/UX Designer
* Responsibilities
1. Project Management:
- Oversaw the full development cycle, including project planning, task delegation, team meetings, and documentation writing.
2.  Environment Design:
- Designed and assembled all game scenes in Unity, integrating and modifying assets from the Unity Asset Store to fit the project's style and requirements.
3. UX/UI Design:
- Created and implemented all in-game dialogue text to enhance narrative engagement.
- Designed and built the main game menu UI for an intuitive user experience.
- Gameplay Flow Improvement:
- Developed seamless scene transitions to ensure a smooth and immersive gameplay experience.


## Table of Contents

- [Evaluation Plan](#evaluation-plan)
- [Evaluation Report](#evaluation-report)
- [Shaders and Special Effects](#shaders-and-special-effects)
- [Summary of Contributions](#summary-of-contributions)
- [References and External Resources](#references-and-external-resources)

## Evaluation Plan
### Evaluation Techniques:
Our team would like to use questionnaires as the querying technique. The main reason is that post-play surveys will mainly collect quantitative and qualitative data from players. Our team would also like to use cooperative evaluation as the observational techniques. Our team chose cooperative evaluation to foster a more interactive and insightful observation.  In the questionnaire survey, we would like to ask players about the whole experience, the detail needed to improve, difficulty, satisfaction, aesthetic appeal, specific gameplay mechanics and understanding of the game etc. After playing the game, participants will complete a survey. During the cooperative evaluation survey, our team would encourage the user to ask questions or express thoughts during the process. Participants will play the game and voice any confusion, difficulties, or thoughts. The evaluator will respond, ask follow-up questions, or guide the participant when necessary. This will help us identify specific moments where players struggle with understanding the mechanics or the narrative.


### Participants:
Our team will looking for participants between 18-40, with different work and education backgrounds, and different levels of gaming experience. All the participants need to confirm they don’t have any PTSD to the mental health issue and comfortable to talk about the suicide and mental health problem theme. All the consequence that cause due to above issues are not our team’s responsibility after participant confirm to take the trial.

### Data Collection
 Our team would collect both quantitative data (Likert scale ratings) and qualitative data (open-ended comments) through the post-play survey. We would like to collect these data through a survey after participants finish playing the game. The Likert scale will be used to rate different aspects of the game. For example, difficulty, enjoyment and emotional engagement etc. Meanwhile, open-ended questions will allow participants to provide detailed feedback on areas like gameplay mechanics, narrative clarity, and aesthetic appeal. Our team plans to use online survey tools such as google forms to contain likert scale ratings and open-ended questions.
	Our team would also collect real-time qualitative data based on player behavior and verbal feedback. This data will include any questions or thoughts voiced by participants as they interact with the game, as well as responses from the evaluator. During gameplay, all the interactions on the screen made with the user are recorded for further analysis. Also, all the questions asked, thoughts, voice as well as the evaluators’ respond, guide and further questions will be documented. The document will also include the specific moments when players encounter challenges or react strongly to certain elements of the game. Our team will use screen recording and audio recording tool to capture the verbal feedback. Notes will be taken and integrated into one document to highlight key insights.

### Data analysis
Our team will calculate the averages and percentage of each level for the likert scale response. Every scene has their unique likert scale that can help us precisely improve the specific scene and details. At the end of each scene’s likert scale, there will be several open-ended questions to ask participants indicating the specific problem found among the playtime. All the open-ended responses will be grouped into common themes. During cooperative evaluation, our team will transcribe and document players’ reactions. Our team will also transcribe any questions and thoughts raising during the sessions into the document. These real-time insights will be analyzed to identify recurring challenges or points where players may need additional guidance or where the game’s design needs clarification. Our team will categorize observations based on the type of feedback.
Three main metrics — Engagement, Difficulty, Understanding, are included in the report. For all the three metrics, we would combine the likert scale, open questionnaires and observing feedback. For engagement, our group will based on players’ Likert scale responses and verbal reactions, we will measure how emotionally connected they felt to the game’s story and characters. For difficulty, our group will evaluate both the Likert scale ratings and observing when players encounter confusion or frustration during cooperative evaluation, we will assess the game’s difficulty balance. For understanding, our group will assess how well players understood the mechanics and narrative, both from the survey responses and real-time feedback gathered during gameplay.

## Evaluation Report

### Feedback Summarization
During the production of the game, there were some situations that were not taken into consideration, so it was decided to find and improve the shortcomings of the game in the form of survey. The game received feedbacks from 12 surveys and used them to improve it, which enhance the overall gameplay experience. First, some scenes are missing player tasks, which leaves players unsure of their objectives. In addition, some players' quest guidelines seem simple and need further hints. A bug with the pause menu has also been identified, which allows players to walk around at will when entering the dialogue phase. Moreover, When the game is in full screen, the q button for dialogue is upwards, which is in the wrong place. A  black background also appears during scene-to-scene transitions. Finally, some scenes were also found to be without music.

### Shaders and Special Effects
#### Partical Effect
Find the partical system here: Scene: Intro: Object: Fog

![fog](https://github.com/user-attachments/assets/3bed72de-0f4b-4195-8c2d-c648c2bf0992)

**Overview:**

- The particle system is designed to create a fog effect. This effect enhances the eerie and immersive ambiance, especially in scenes that require a mysterious or suspenseful mood. The system is located in the “Intro” scene under the GameObject “Fog” response for simulating a subtle, misty fog that drifts through the room.

**Attributes:**

- By using fog image to create a fog material and custormized the effect under inspector.

- **Particle Count**: The particle count is set to **30**, creating a balanced visual effect that simulates fog without overwhelming the scene or causing performance issues.

- **Shape**: The particle system emits from a **Box** shape, which is set to fill the room's volume.

- **Start Size & Size Over Lifetime**: Particles start small and gradually increase in size over their lifetime. This simulates the natural dispersal and movement of fog, creating an illusion that the fog is thickening as it moves through the room.

- **Start Speed & Speed Range**: To introduce randomness, the speed of the fog particles is set to vary between **0.1 and 0.1**, ensuring that the fog moves slowly and naturally. This subtle variation makes the effect feel organic rather than mechanical, with particles drifting at slightly different speeds.

- **Color Over Lifetime**: The fog's color is set to a soft gray, and it remains consistent over its lifetime to simulate realistic fog behavior.

- **Rotation and Randomized Rotation**: The particles have randomized rotation applied to them, which ensures that they don’t all move in the same way.

- **Gravity Modifier**: Gravity was turned off, allowing the fog to remain suspended in the air, as fog would behave in an enclosed environment.

- **Lifetime**: The particle's lifetime is set to a relatively long duration to keep the fog hanging in the air for a while before dissipating.

**Randomness:**

- **Speed Variation**: The fog particles have a slight variation in speed, preventing uniform movement.

- **Rotation**: Each particle's rotation is randomized to avoid repetitive patterns and ensure that the fog appears organic and fluid.

- **Size Over Lifetime**: The fog particles grow at different rates, creating an effect where the fog appears to be swirling or gathering in certain areas. This randomness makes the environment feel more unpredictable and engaging for the player.

#### Shader 1
Find the erosion shader for the game [here](Assets/Material/Yue%20Man/MirrorShader)

![erosion](https://github.com/user-attachments/assets/df4aa9de-8e22-44ab-ba32-3272348203ea)

**Overview:**

- The erosion shader using a custom fragment shader to achieve a dynamic reveal effect at the end screen on the main character. This shader creates the appearance of the object being gradually exposed or eroded over time.

**Explanation:**

- The erosion shader works within Unity's rendering pipeline by manipulating the fragment stage of the pipeline. At this stage, after the object’s vertices have been processed and transformed in the vertex shader, the fragment shader determines the color of each pixel that gets rendered on the screen. The erosion shader utilizes a mask texture and a time-based `_RevealValue` parameter to blend between the object’s original texture and the erosion color, which simulates an erosion effect as the value gradually increases.

- The shader uses properties like `_MainTex` for the main texture and `_MaskTex` for the mask texture. As the `_RevealValue` changes over time, it gradually reveals parts of the object using the mask's grayscale values. This technique effectively creates a procedural reveal effect, which is perfect for dynamic visuals.

- **`_SrcFactor`** and **`_DstFactor`**: These are blending mode parameters that specify how the source and destination colors are blended. They determine how the source (the object being rendered) and the destination (the background or other objects) are mixed.

- **`_MainTex`**: The main texture displayed on the surface of the object.

- **`_MaskTex`**: Mask texture used to control the erosion effect.

- **`_RevealValue`**: A float value that controls the progress of the erosion effect. It starts at 0 (completely hidden) and goes up to 1 (fully revealed).

- **`_Feather`**: Used for smoothing transitions. It controls the softness of the edges of the revealed areas.

- **`_EroColor`**: Defines the color of the erosion effect.

The shader script [link](Assets/Material/Yue%20Man/MirrorShader/PillShador.shader)

**C# script**

- To control the dynamic behavior of the erosion effect, a C# script called `ErosionController` is used to update the shader’s `_RevealValue` in real-time. The script increases the value of `_RevealValue` as time progresses, thereby animating the erosion effect. This script is crucial for managing the interaction between the gameplay and the shader parameters, as it controls how fast and when the erosion happens.
The script [link](Assets/Scripts/Yue%20Man/ErosionController.cs)

**Material setup**

- The main character at the end screen use this shader. We reference the erosion shader and contains parameter such as erosion color, feather and reveal. These can be customized directly in the inspector. The material is applied to game objects that need to exhibit the erosion effect. This material acts as a bridge between the shader and the Unity rendering system, enabling real-time visual updates based on the player’s interaction or game events.

#### Shader 2

The [Post Processing shader](Assets/Material/Khai%20Nguy/PostProcessing.shader) is a shader for post-processing effects including 2 passes of 2 different effects [vignette](#vignette) and [distortion](#distortion).

<ins><b id="vignette">Pulsating Vignette</b></ins>

The pulsating vignette effect is the first `pass` in the [Post Processing shader file](Assets/Material/Khai%20Nguy/PostProcessing.shader).

![vignette](https://github.com/user-attachments/assets/fa5bf244-abd5-4e1e-a59f-14aac26cadc0)

**Overview**

The pulsating vignette effect is used to create a stylised atmospheric feel to the game or to signify the character is low on health as it envelops an oval area around the screen, creating a tunneling vision and combine with a pulsating effect (making the vignette appear and disappear).

**Explanation**

The vignette effect works within Unity's rendering pipeline by manipulating the fragment stage of the pipeline. At this stage, after the object’s vertices have been processed and transformed in the vertex shader, the fragment shader calculates an oval of with the size of `_Radius` with the color `_Tint` and the length `_Feather` of gradient transition area from the color `_Tint` to `_MainTex`. The `_Frequency` is used to determine the speed of which the vignette is visible and invisible through `sin` of the value of `_Time.y` - which is the time since level load, creating a pulsating effect.

- **`_MainTex`** is the main texture displayed to the screen.
- **`_Radius`** is the radius of the effect.
- **`_Feather`** is the length of the gradient transition area.
- **`_Frequency`** is the speed of the pulsating effect.
- **`_Tint`** is the color of the effect.

<ins><b id="distortion">Distortion</b></ins>

The distortion effect is the second `pass` in the [Post Processing shader file](Assets/Material/Khai%20Nguy/PostProcessing.shader).

![distortion](https://github.com/user-attachments/assets/3f5fbc9f-6e47-476d-ab3a-10eb7d912421)

**Overview**

The distortion effect is used to create a rippling effect on-screen to describe the uneasiness and the unstable state of the main character in the last minigame.

**Explanation**

The distortion effect works within Unity's rendering pipeline by manipulating the fragment stage of the pipeline. At this stage, after the object’s vertices have been processed and transformed in the vertex shader, the fragment shader calls the `Distort` function to calculate the new position of the pixel that gets rendered on the screen based on the combination of multiple `sin` waves. This shader utilizes the `_Time` value to create a continuous displacement of the pixels.

- **`_MainTex`** is the main texture displayed to the screen.
- **`_Scale`** is a `float` value for the `uv` displacement before applying the `sin` waves which defaults to `1`.
- **`_TimeScale`** is a `float` value to scale or increase the value of `_Time` affecting the speed of the `sin` waves effect which defaults to `2`.

<ins><b>Setup</b></ins>

**C# Script**

To apply the post-processing effect we can attach the [`PostEffectsController`](Assets/Scripts/Khai%20Nguy/PostEffectsController.cs) to the camera. The script then can be used to set the effects `On` or `Off` and
- the `_Radius`, `_Feather`, `_Tint` values of the [vignette](#vignette) effect, altering the looks of the effect.
- the `_Scale` and `_TimeScale` values of the [distortion](#distortion) effect, altering the speed and intensity of the effect.


### References and External Resources
1. Utilized various assets downloaded from the Unity Asset Store, including 3D models, environment elements, sound effects, and tools, to enhance the project's content and functionality.
2. Employed ProBuilder for 3D modeling, Polybrush for direct editing and sculpting of models, and ProGrids for precise object placement within the scene.
3. Used a skybox from Jan Mróz, available at [GitLab - Stars Skybox Shader](https://gitlab.com/janmroz97/stars-skybox-shader/).
4. Used ChatGPT to generate game concept art for the Game Design Document (GDD).
5. Used ChatGPT for some images.
6. Music and Sound Effects from:
    - [Free Sound](https://freesound.org/)
    - [Sound Snap](https://www.soundsnap.com/)
    - [Pixabay](https://pixabay.com/)
    - Goth Cry - "Hopeless City" (RE3 Save Room) ~Resident Evil 3 Nemesis 1999: Self-Made OST(https://www.youtube.com/watch?v=OaSthsbgf40)
    - Resident Evil Remake Save Room (Safe Heaven)(https://www.youtube.com/watch?v=4pZV3UPmXI4&list=RD4pZV3UPmXI4&start_radio=1&t=41s)
    - Environmental Sound (Silence 2) - Metroid Fusion(https://www.youtube.com/watch?v=GjsE4AOb4dQ&t=3390s)
7. Icons from:
    - [Flaticon](https://www.flaticon.com/)
    - [uxwing](https://uxwing.com/)
