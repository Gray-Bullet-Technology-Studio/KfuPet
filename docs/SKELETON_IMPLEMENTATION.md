# KfuPet 骨骼动画系统 - 实施规范

> v2.0 | 架构优化版

## 目录

1. [实施步骤](#一实施步骤)
2. [项目架构设计](#二项目架构设计)
3. [角色资源结构](#三角色资源结构)
4. [核心模块设计](#四核心模块设计)
5. [配置文件格式](#五配置文件格式)
6. [命名规范](#六命名规范)
7. [注意事项](#七注意事项)

---

## 一、实施步骤

### Phase 1: 核心数据模型

**目标**：定义骨骼、附件、角色的基础数据结构。

**关键任务**：

| 序号 | 任务 | 描述 | 输出文件 |
|------|------|------|----------|
| 1.1 | 定义 `Bone` 类 | 骨骼节点模型，包含位置、旋转、缩放、父骨骼引用 | `Models/Bone.cs` |
| 1.2 | 定义 `Skeleton` 类 | 骨骼系统管理器，负责骨骼树构建 | `Models/Skeleton.cs` |
| 1.3 | 定义 `Attachment` 类 | 附件绑定模型，支持多资源切换 | `Models/Attachment.cs` |
| 1.4 | 定义 `AttachmentSet` 类 | 附件资源集合，管理可切换资源 | `Models/AttachmentSet.cs` |
| 1.5 | 定义 `Character` 类 | 角色整体模型，聚合骨骼、附件、表情 | `Models/Character.cs` |
| 1.6 | 定义 `Expression` 类 | 表情模型，管理表情切换规则 | `Models/Expression.cs` |

**验收标准**：
- 骨骼层级关系正确建立
- 一根骨骼支持多个附件
- 附件支持多资源切换

---

### Phase 2: 配置加载服务

**目标**：实现从配置文件加载角色数据的服务层。

**关键任务**：

| 序号 | 任务 | 描述 | 输出文件 |
|------|------|------|----------|
| 2.1 | 实现 `CharacterLoader` | 加载角色基本信息 | `Services/CharacterLoader.cs` |
| 2.2 | 实现 `SkeletonLoader` | 加载骨骼定义 | `Services/SkeletonLoader.cs` |
| 2.3 | 实现 `AttachmentLoader` | 加载附件配置 | `Services/AttachmentLoader.cs` |
| 2.4 | 实现 `ExpressionLoader` | 加载表情配置 | `Services/ExpressionLoader.cs` |
| 2.5 | 实现 `TextureLoader` | 加载图片资源 | `Services/TextureLoader.cs` |
| 2.6 | 实现 `ResourceManager` | 资源缓存与管理 | `Services/ResourceManager.cs` |

**验收标准**：
- 所有配置文件正确加载
- 资源缓存机制生效
- 加载失败时有优雅降级

---

### Phase 3: 渲染引擎

**目标**：实现骨骼和附件的渲染系统。

**关键任务**：

| 序号 | 任务 | 描述 | 输出文件 |
|------|------|------|----------|
| 3.1 | 定义 `RenderContext` | 渲染上下文，管理渲染状态 | `Core/Rendering/RenderContext.cs` |
| 3.2 | 定义 `Renderer` 基类 | 渲染器抽象基类 | `Core/Rendering/Renderer.cs` |
| 3.3 | 实现 `SkeletonRenderer` | 骨骼结构渲染 | `Core/Rendering/SkeletonRenderer.cs` |
| 3.4 | 实现 `AttachmentRenderer` | 附件图片渲染 | `Core/Rendering/AttachmentRenderer.cs` |
| 3.5 | 实现 `WpfRenderer` | WPF 渲染实现 | `Core/Rendering/WpfRenderer.cs` |
| 3.6 | 实现 ZOrder 管理 | 控制渲染层级 | `Core/Rendering/ZOrderManager.cs` |

**验收标准**：
- 骨骼结构正确渲染
- 附件图片正确绑定
- 渲染层级正确（遮挡关系）

---

### Phase 4: 核心算法

**目标**：实现变换计算、插值等核心算法。

**关键任务**：

| 序号 | 任务 | 描述 | 输出文件 |
|------|------|------|----------|
| 4.1 | 实现 `Matrix3x3` | 3x3 变换矩阵 | `Core/Math/Matrix3x3.cs` |
| 4.2 | 实现 `Transform` | 变换计算工具 | `Core/Math/Transform.cs` |
| 4.3 | 实现 `Interpolator` | 插值算法（线性、贝塞尔） | `Core/Math/Interpolator.cs` |
| 4.4 | 实现 `Bezier` | 贝塞尔曲线计算 | `Core/Math/Bezier.cs` |
| 4.5 | 实现骨骼变换计算 | 层级变换、世界坐标计算 | `Core/Skeleton/SkeletonTransform.cs` |

**验收标准**：
- 变换矩阵计算正确
- 插值效果平滑自然
- 骨骼联动变换正常

---

### Phase 5: 动画系统

**目标**：实现动画数据结构、加载和播放。

**关键任务**：

| 序号 | 任务 | 描述 | 输出文件 |
|------|------|------|----------|
| 5.1 | 定义 `Animation` 类 | 动画数据结构 | `Models/Animation.cs` |
| 5.2 | 定义 `Keyframe` 类 | 关键帧数据 | `Models/Keyframe.cs` |
| 5.3 | 实现 `AnimationLoader` | 加载动画数据 | `Services/AnimationLoader.cs` |
| 5.4 | 实现 `AnimationPlayer` | 动画播放控制器 | `Core/Animation/AnimationPlayer.cs` |
| 5.5 | 实现动画混合 | 多动画叠加混合 | `Core/Animation/AnimationBlender.cs` |

**验收标准**：
- 动画流畅播放
- 关键帧插值正确
- 播放控制功能正常

---

### Phase 6: 状态机与表情系统

**目标**：实现动画状态管理和表情切换。

**关键任务**：

| 序号 | 任务 | 描述 | 输出文件 |
|------|------|------|----------|
| 6.1 | 定义 `AnimationState` 枚举 | 动画状态类型 | `Models/AnimationState.cs` |
| 6.2 | 实现 `AnimationStateMachine` | 状态机管理器 | `Core/Animation/AnimationStateMachine.cs` |
| 6.3 | 实现表情切换 | 根据状态切换附件资源 | `Core/Character/ExpressionController.cs` |
| 6.4 | 实现触发条件 | 状态切换的触发规则 | `Core/Animation/StateTrigger.cs` |

**验收标准**：
- 动画状态正确切换
- 表情切换平滑自然
- 状态过渡效果正确

---

## 二、项目架构设计

### 目录结构总览

```
KfuPet/
├── Assets/                    # 应用级静态资源
│   └── icon/                  # 图标资源
│       ├── app.ico
│       ├── tray.ico
│       └── Startlogo.png
│
├── Characters/                # 角色资源目录（按角色名组织）
│   └── KfuPet/                # 角色文件夹
│       ├── character.json     # 角色基本信息
│       ├── skeleton.json      # 骨骼定义
│       ├── attachments.json   # 附件绑定配置
│       ├── expressions.json   # 表情配置
│       ├── physics.json       # 物理参数（后期）
│       ├── images/            # 图片资源
│       │   ├── head/
│       │   │   ├── normal.png
│       │   │   ├── happy.png
│       │   │   └── angry.png
│       │   ├── body/
│       │   ├── arm_left/
│       │   ├── arm_right/
│       │   ├── leg_left/
│       │   └── leg_right/
│       ├── animations/        # 动画数据目录
│       │   ├── idle.json
│       │   ├── walk.json
│       │   ├── jump.json
│       │   └── wave.json
│       ├── audio/             # 音频资源（后期）
│       └── thumbnails/        # 缩略图（后期）
│
├── Config/                    # 应用全局配置
│   ├── Version.json           # 版本配置
│   ├── Settings.json          # 应用设置（后期）
│   ├── AI.json                # AI 配置（后期）
│   ├── Theme.json             # 主题配置（后期）
│   └── Log.json               # 日志配置（后期）
│
├── Core/                      # 核心算法层
│   ├── Animation/             # 动画核心
│   │   ├── AnimationPlayer.cs
│   │   ├── AnimationBlender.cs
│   │   ├── AnimationStateMachine.cs
│   │   └── StateTrigger.cs
│   ├── Character/             # 角色核心
│   │   ├── ExpressionController.cs
│   │   └── CharacterController.cs
│   ├── Math/                  # 数学工具
│   │   ├── Matrix3x3.cs
│   │   ├── Transform.cs
│   │   ├── Interpolator.cs
│   │   └── Bezier.cs
│   ├── Physics/               # 物理系统（后期）
│   │   ├── PhysicsEngine.cs
│   │   └── PhysicsBone.cs
│   ├── Rendering/             # 渲染引擎
│   │   ├── Renderer.cs
│   │   ├── RenderContext.cs
│   │   ├── SkeletonRenderer.cs
│   │   ├── AttachmentRenderer.cs
│   │   ├── WpfRenderer.cs
│   │   └── ZOrderManager.cs
│   ├── Skeleton/              # 骨骼核心
│   │   └── SkeletonTransform.cs
│   ├── AI/                    # AI 系统（后期）
│   └── Memory/                # 记忆系统（后期）
│
├── Models/                    # 数据模型
│   ├── Animation.cs           # 动画数据模型
│   ├── AnimationState.cs      # 动画状态枚举
│   ├── Attachment.cs          # 附件绑定模型
│   ├── AttachmentSet.cs       # 附件资源集合
│   ├── Bone.cs                # 骨骼节点模型
│   ├── Character.cs           # 角色模型
│   ├── Expression.cs          # 表情模型
│   ├── Keyframe.cs            # 关键帧模型
│   ├── Skeleton.cs            # 骨骼系统模型
│   ├── PhysicsBone.cs         # 物理骨骼（后期）
│   └── BoneConstraint.cs      # 骨骼约束（后期）
│
├── Services/                  # 服务层
│   ├── AnimationLoader.cs     # 动画加载服务
│   ├── AttachmentLoader.cs    # 附件加载服务
│   ├── CharacterLoader.cs     # 角色加载服务
│   ├── ExpressionLoader.cs    # 表情加载服务
│   ├── ResourceManager.cs     # 资源管理服务
│   ├── SkeletonLoader.cs      # 骨骼加载服务
│   └── TextureLoader.cs       # 纹理加载服务
│
├── ViewModels/                # 视图模型（MVVM）
│   ├── MainViewModel.cs
│   └── CharacterViewModel.cs
│
├── Views/                     # 窗口视图
│   ├── MainWindow.xaml
│   ├── MainWindow.xaml.cs
│   ├── SplashWindow.xaml
│   └── SplashWindow.xaml.cs
│
├── Controls/                  # WPF 自定义控件
│   ├── CharacterCanvas.xaml   # 角色渲染画布控件
│   ├── CharacterCanvas.xaml.cs
│   └── ...
│
├── Helpers/                   # 通用工具类
│   ├── JsonHelper.cs          # JSON 序列化工具
│   ├── PathHelper.cs          # 路径处理工具
│   └── LogHelper.cs           # 日志工具（后期）
│
├── App.xaml
├── App.xaml.cs
├── GlobalUsings.cs
├── AssemblyInfo.cs
├── KfuPet.csproj
└── KfuPet.slnx
```

### 各目录职责说明

| 目录 | 职责 | 说明 |
|------|------|------|
| **Assets/** | 应用级静态资源 | 图标、启动画面等，不随角色变化 |
| **Characters/** | 角色资源包 | 按角色名组织，包含图片、配置、动画 |
| **Config/** | 应用全局配置 | 版本、设置、AI、主题等配置文件 |
| **Core/** | 核心算法层 | 动画、渲染、物理、AI 等核心引擎 |
| **Models/** | 数据模型 | 纯数据结构，无业务逻辑 |
| **Services/** | 服务层 | 资源加载、缓存管理等业务服务 |
| **ViewModels/** | 视图模型 | MVVM 模式下的 UI 数据绑定层 |
| **Views/** | 窗口视图 | WPF 窗口和页面 |
| **Controls/** | WPF 自定义控件 | 可复用的 UI 组件 |
| **Helpers/** | 通用工具类 | 无状态的辅助方法 |

### 核心层（Core）设计

**Core 层是架构的核心，包含所有算法和引擎：**

```
Core/
├── Animation/          # 动画引擎
│   ├── AnimationPlayer     # 动画播放控制
│   ├── AnimationBlender    # 动画混合
│   ├── AnimationStateMachine # 状态机
│   └── StateTrigger        # 状态触发器
│
├── Character/           # 角色控制
│   ├── ExpressionController # 表情控制器
│   └── CharacterController  # 角色主控制器
│
├── Math/               # 数学工具
│   ├── Matrix3x3           # 3x3 矩阵
│   ├── Transform           # 变换计算
│   ├── Interpolator        # 插值算法
│   └── Bezier              # 贝塞尔曲线
│
├── Physics/            # 物理引擎（后期）
│   ├── PhysicsEngine       # 物理引擎核心
│   └── PhysicsBone         # 物理骨骼
│
├── Rendering/          # 渲染引擎
│   ├── Renderer            # 渲染器基类
│   ├── RenderContext       # 渲染上下文
│   ├── SkeletonRenderer    # 骨骼渲染
│   ├── AttachmentRenderer  # 附件渲染
│   ├── WpfRenderer         # WPF 实现
│   └── ZOrderManager       # 层级管理
│
├── Skeleton/           # 骨骼核心算法
│   └── SkeletonTransform   # 骨骼变换计算
│
├── AI/                 # AI 系统（后期）
└── Memory/             # 记忆系统（后期）
```

---

## 三、角色资源结构

### 推荐结构

```
Characters/KfuPet/
├── character.json      # 角色基本信息
├── skeleton.json       # 骨骼定义
├── attachments.json    # 附件绑定配置
├── expressions.json    # 表情配置
├── physics.json        # 物理参数（后期）
├── images/             # 图片资源
│   ├── head/
│   │   ├── normal.png
│   │   ├── happy.png
│   │   ├── angry.png
│   │   ├── sad.png
│   │   └── sleep.png
│   ├── body/
│   │   ├── default.png
│   │   └── outfit_summer.png
│   ├── arm_left/
│   ├── arm_right/
│   ├── leg_left/
│   ├── leg_right/
│   ├── hair/
│   │   ├── default.png
│   │   └── long.png
│   ├── eyes/
│   │   ├── normal.png
│   │   ├── blink.png
│   │   └── wink.png
│   └── accessories/
│       ├── glasses.png
│       └── hat.png
├── animations/         # 动画数据
│   ├── idle.json
│   ├── walk.json
│   ├── jump.json
│   ├── wave.json
│   └── breathe.json
├── audio/              # 音频资源（后期）
└── thumbnails/         # 缩略图（后期）
```

### 目录职责说明

| 文件/目录 | 职责 |
|-----------|------|
| `character.json` | 角色名称、版本、作者、描述等基本信息 |
| `skeleton.json` | 骨骼层级结构、初始位置、旋转、缩放 |
| `attachments.json` | 附件与骨骼的绑定关系、锚点、偏移、层级 |
| `expressions.json` | 表情定义、表情与附件资源的映射 |
| `physics.json` | 物理参数、碰撞体、约束（后期） |
| `images/` | 身体部位图片，按部位分目录，支持多资源切换 |
| `animations/` | 动画关键帧数据，按动画名称分文件 |
| `audio/` | 语音、音效资源（后期） |
| `thumbnails/` | 角色预览缩略图（后期） |

---

## 四、核心模块设计

### 4.1 骨骼系统

**Bone 模型结构：**

```
Bone
├── Id: string                    # 骨骼唯一标识
├── Name: string                  # 骨骼名称
├── ParentId: string?             # 父骨骼 ID
├── Parent: Bone?                 # 父骨骼引用
├── Children: List<Bone>          # 子骨骼列表
├── LocalPosition: Point          # 相对于父骨骼的位置
├── LocalRotation: double         # 相对于父骨骼的旋转（弧度）
├── LocalScale: Vector            # 相对于父骨骼的缩放
├── WorldTransform: Matrix3x3     # 世界变换矩阵（派生）
├── Attachments: List<Attachment> # 绑定的附件列表
└── IsActive: bool                # 是否激活
```

**关键点：一根骨骼可以绑定多个 Attachment**

### 4.2 附件系统

**Attachment 模型结构：**

```
Attachment
├── Id: string                       # 附件唯一标识
├── BoneId: string                   # 绑定的骨骼 ID
├── Bone: Bone?                      # 绑定的骨骼引用
├── Name: string                     # 附件名称（如 "Hair", "Eyes", "Glasses"）
├── Set: AttachmentSet               # 附件资源集合
├── CurrentResource: string          # 当前使用的资源 ID
├── Offset: Point                    # 相对于骨骼原点的偏移
├── Pivot: Point                     # 旋转/缩放中心（0~1）
├── ZOrder: int                      # 渲染层级
├── Visible: bool                    # 是否可见
└── Transform: Transform             # 附加变换
```

**AttachmentSet 模型结构：**

```
AttachmentSet
├── Resources: Dictionary<string, string>  # 资源 ID → 图片路径
├── DefaultResource: string               # 默认资源 ID
└── CurrentResourceId: string             # 当前资源 ID
```

**多附件绑定示例：**

```
Head Bone
├── Attachment: "Face"
│   ├── Resources: {"normal", "happy", "angry", "sad", "sleep"}
│   └── Current: "normal"
├── Attachment: "Hair"
│   ├── Resources: {"default", "long", "short"}
│   └── Current: "default"
├── Attachment: "Eyes"
│   ├── Resources: {"normal", "blink", "wink"}
│   └── Current: "normal"
└── Attachment: "Glasses"
    ├── Resources: {"default", "sunglasses"}
    └── Current: null (未佩戴)
```

### 4.3 表情系统

**Expression 模型结构：**

```
Expression
├── Id: string                    # 表情唯一标识
├── Name: string                  # 表情名称（如 "Happy", "Angry", "Sleep"）
├── Trigger: ExpressionTrigger    # 触发条件
├── Targets: List<ExpressionTarget> # 目标附件及资源
└── Duration: TimeSpan?           # 持续时间（null 表示保持直到切换）
```

**ExpressionTarget 结构：**

```
ExpressionTarget
├── AttachmentId: string          # 目标附件 ID
├── ResourceId: string            # 目标资源 ID
└── BlendDuration: TimeSpan       # 混合过渡时间
```

### 4.4 动画系统

**Animation 模型结构：**

```
Animation
├── Id: string                    # 动画唯一标识
├── Name: string                  # 动画名称
├── Duration: double              # 动画时长（秒）
├── Fps: int                      # 帧率
├── Loop: bool                    # 是否循环
├── Keyframes: List<Keyframe>     # 关键帧列表
└── Events: List<AnimationEvent>  # 动画事件（后期）
```

**Keyframe 模型结构：**

```
Keyframe
├── Time: double                  # 时间点（秒）
├── BoneStates: Dictionary<string, BoneState>  # 骨骼状态
└── AttachmentStates: Dictionary<string, string>  # 附件资源切换
```

**BoneState 结构：**

```
BoneState
├── Position: Point?              # 位置（null 表示继承上一帧）
├── Rotation: double?             # 旋转（null 表示继承上一帧）
├── Scale: Vector?                # 缩放（null 表示继承上一帧）
└── Interpolation: InterpolationType  # 插值类型
```

### 4.5 渲染系统

**Renderer 抽象基类：**

```
Renderer
├── RenderContext: RenderContext  # 渲染上下文
├── Render(skeleton)              # 渲染骨骼
├── Render(attachment)            # 渲染附件
└── Clear()                       # 清空画布
```

**RenderContext 结构：**

```
RenderContext
├── Canvas: Canvas                # 渲染画布
├── DpiScale: (double, double)    # DPI 缩放因子
├── Camera: Camera                # 相机（后期）
└── Lights: List<Light>           # 灯光（后期）
```

---

## 五、配置文件格式

### 5.1 character.json

**文件路径**：`Characters/{CharacterName}/character.json`

```json
{
  "id": "kfupet",
  "name": "KfuPet",
  "displayName": "KfuPet",
  "version": "1.0.0",
  "author": "Gray Bullet Technology Studio",
  "description": "AI-powered desktop companion",
  "tags": ["ai", "desktop", "pet"],
  "defaultAnimation": "idle",
  "defaultExpression": "normal",
  "canvasSize": { "width": 512, "height": 768 },
  "origin": { "x": 256, "y": 768 }
}
```

### 5.2 skeleton.json

**文件路径**：`Characters/{CharacterName}/skeleton.json`

```json
{
  "bones": [
    {
      "id": "root",
      "name": "Root",
      "parentId": null,
      "position": { "x": 256, "y": 768 },
      "rotation": 0,
      "scale": { "x": 1, "y": 1 }
    },
    {
      "id": "body",
      "name": "Body",
      "parentId": "root",
      "position": { "x": 0, "y": -268 },
      "rotation": 0,
      "scale": { "x": 1, "y": 1 }
    },
    {
      "id": "head",
      "name": "Head",
      "parentId": "body",
      "position": { "x": 0, "y": -200 },
      "rotation": 0,
      "scale": { "x": 1, "y": 1 }
    },
    {
      "id": "arm_left",
      "name": "LeftArm",
      "parentId": "body",
      "position": { "x": -106, "y": -50 },
      "rotation": 0,
      "scale": { "x": 1, "y": 1 }
    },
    {
      "id": "arm_right",
      "name": "RightArm",
      "parentId": "body",
      "position": { "x": 106, "y": -50 },
      "rotation": 0,
      "scale": { "x": 1, "y": 1 }
    },
    {
      "id": "leg_left",
      "name": "LeftLeg",
      "parentId": "body",
      "position": { "x": -56, "y": 150 },
      "rotation": 0,
      "scale": { "x": 1, "y": 1 }
    },
    {
      "id": "leg_right",
      "name": "RightLeg",
      "parentId": "body",
      "position": { "x": 56, "y": 150 },
      "rotation": 0,
      "scale": { "x": 1, "y": 1 }
    }
  ]
}
```

### 5.3 attachments.json

**文件路径**：`Characters/{CharacterName}/attachments.json`

```json
{
  "attachments": [
    {
      "id": "face",
      "boneId": "head",
      "name": "Face",
      "offset": { "x": 0, "y": 0 },
      "pivot": { "x": 0.5, "y": 0.5 },
      "zOrder": 5,
      "visible": true,
      "resources": {
        "normal": "images/head/normal.png",
        "happy": "images/head/happy.png",
        "angry": "images/head/angry.png",
        "sad": "images/head/sad.png",
        "sleep": "images/head/sleep.png"
      },
      "defaultResource": "normal"
    },
    {
      "id": "hair",
      "boneId": "head",
      "name": "Hair",
      "offset": { "x": 0, "y": -20 },
      "pivot": { "x": 0.5, "y": 1 },
      "zOrder": 6,
      "visible": true,
      "resources": {
        "default": "images/hair/default.png",
        "long": "images/hair/long.png",
        "short": "images/hair/short.png"
      },
      "defaultResource": "default"
    },
    {
      "id": "eyes",
      "boneId": "head",
      "name": "Eyes",
      "offset": { "x": 0, "y": 5 },
      "pivot": { "x": 0.5, "y": 0.5 },
      "zOrder": 4,
      "visible": true,
      "resources": {
        "normal": "images/eyes/normal.png",
        "blink": "images/eyes/blink.png",
        "wink": "images/eyes/wink.png"
      },
      "defaultResource": "normal"
    },
    {
      "id": "body",
      "boneId": "body",
      "name": "Body",
      "offset": { "x": 0, "y": 0 },
      "pivot": { "x": 0.5, "y": 0.5 },
      "zOrder": 2,
      "visible": true,
      "resources": {
        "default": "images/body/default.png",
        "outfit_summer": "images/body/outfit_summer.png"
      },
      "defaultResource": "default"
    },
    {
      "id": "arm_left",
      "boneId": "arm_left",
      "name": "LeftArm",
      "offset": { "x": 0, "y": 0 },
      "pivot": { "x": 0.8, "y": 0.2 },
      "zOrder": 3,
      "visible": true,
      "resources": {
        "default": "images/arm_left/default.png"
      },
      "defaultResource": "default"
    },
    {
      "id": "arm_right",
      "boneId": "arm_right",
      "name": "RightArm",
      "offset": { "x": 0, "y": 0 },
      "pivot": { "x": 0.2, "y": 0.2 },
      "zOrder": 3,
      "visible": true,
      "resources": {
        "default": "images/arm_right/default.png"
      },
      "defaultResource": "default"
    },
    {
      "id": "leg_left",
      "boneId": "leg_left",
      "name": "LeftLeg",
      "offset": { "x": 0, "y": 0 },
      "pivot": { "x": 0.5, "y": 0 },
      "zOrder": 1,
      "visible": true,
      "resources": {
        "default": "images/leg_left/default.png"
      },
      "defaultResource": "default"
    },
    {
      "id": "leg_right",
      "boneId": "leg_right",
      "name": "RightLeg",
      "offset": { "x": 0, "y": 0 },
      "pivot": { "x": 0.5, "y": 0 },
      "zOrder": 1,
      "visible": true,
      "resources": {
        "default": "images/leg_right/default.png"
      },
      "defaultResource": "default"
    },
    {
      "id": "glasses",
      "boneId": "head",
      "name": "Glasses",
      "offset": { "x": 0, "y": 10 },
      "pivot": { "x": 0.5, "y": 0.5 },
      "zOrder": 7,
      "visible": false,
      "resources": {
        "default": "images/accessories/glasses.png",
        "sunglasses": "images/accessories/sunglasses.png"
      },
      "defaultResource": null
    }
  ]
}
```

### 5.4 expressions.json

**文件路径**：`Characters/{CharacterName}/expressions.json`

```json
{
  "expressions": [
    {
      "id": "normal",
      "name": "Normal",
      "targets": [
        { "attachmentId": "face", "resourceId": "normal", "blendDuration": 0.2 },
        { "attachmentId": "eyes", "resourceId": "normal", "blendDuration": 0.1 }
      ],
      "duration": null
    },
    {
      "id": "happy",
      "name": "Happy",
      "targets": [
        { "attachmentId": "face", "resourceId": "happy", "blendDuration": 0.3 },
        { "attachmentId": "eyes", "resourceId": "normal", "blendDuration": 0.1 }
      ],
      "duration": null
    },
    {
      "id": "angry",
      "name": "Angry",
      "targets": [
        { "attachmentId": "face", "resourceId": "angry", "blendDuration": 0.3 },
        { "attachmentId": "eyes", "resourceId": "normal", "blendDuration": 0.1 }
      ],
      "duration": null
    },
    {
      "id": "sad",
      "name": "Sad",
      "targets": [
        { "attachmentId": "face", "resourceId": "sad", "blendDuration": 0.3 },
        { "attachmentId": "eyes", "resourceId": "normal", "blendDuration": 0.1 }
      ],
      "duration": null
    },
    {
      "id": "sleep",
      "name": "Sleep",
      "targets": [
        { "attachmentId": "face", "resourceId": "sleep", "blendDuration": 0.5 },
        { "attachmentId": "eyes", "resourceId": "normal", "blendDuration": 0.1 }
      ],
      "duration": null
    },
    {
      "id": "blink",
      "name": "Blink",
      "targets": [
        { "attachmentId": "eyes", "resourceId": "blink", "blendDuration": 0.05 }
      ],
      "duration": "0.15",
      "revertExpression": "normal"
    },
    {
      "id": "wink",
      "name": "Wink",
      "targets": [
        { "attachmentId": "eyes", "resourceId": "wink", "blendDuration": 0.1 }
      ],
      "duration": "0.5",
      "revertExpression": "normal"
    }
  ]
}
```

### 5.5 动画数据格式

**文件路径**：`Characters/{CharacterName}/animations/{animationName}.json`

```json
{
  "id": "idle",
  "name": "Idle",
  "description": "Idle breathing animation",
  "duration": 2.0,
  "fps": 30,
  "loop": true,
  
  "keyframes": [
    {
      "time": 0.0,
      "bones": {
        "head": { "rotation": 0, "position": { "x": 0, "y": 0 } },
        "arm_left": { "rotation": -0.2, "position": { "x": 0, "y": 0 } },
        "arm_right": { "rotation": 0.2, "position": { "x": 0, "y": 0 } },
        "leg_left": { "rotation": 0.1, "position": { "x": 0, "y": 0 } },
        "leg_right": { "rotation": -0.1, "position": { "x": 0, "y": 0 } }
      }
    },
    {
      "time": 0.5,
      "bones": {
        "head": { "rotation": 0.1, "position": { "x": 0, "y": -5 } },
        "arm_left": { "rotation": -0.1, "position": { "x": 0, "y": 0 } },
        "arm_right": { "rotation": 0.1, "position": { "x": 0, "y": 0 } },
        "leg_left": { "rotation": -0.1, "position": { "x": 0, "y": 0 } },
        "leg_right": { "rotation": 0.1, "position": { "x": 0, "y": 0 } }
      }
    },
    {
      "time": 1.0,
      "bones": {
        "head": { "rotation": 0, "position": { "x": 0, "y": 0 } },
        "arm_left": { "rotation": -0.2, "position": { "x": 0, "y": 0 } },
        "arm_right": { "rotation": 0.2, "position": { "x": 0, "y": 0 } },
        "leg_left": { "rotation": 0.1, "position": { "x": 0, "y": 0 } },
        "leg_right": { "rotation": -0.1, "position": { "x": 0, "y": 0 } }
      }
    },
    {
      "time": 1.5,
      "bones": {
        "head": { "rotation": -0.1, "position": { "x": 0, "y": 5 } },
        "arm_left": { "rotation": -0.1, "position": { "x": 0, "y": 0 } },
        "arm_right": { "rotation": 0.1, "position": { "x": 0, "y": 0 } },
        "leg_left": { "rotation": -0.1, "position": { "x": 0, "y": 0 } },
        "leg_right": { "rotation": 0.1, "position": { "x": 0, "y": 0 } }
      }
    },
    {
      "time": 2.0,
      "bones": {
        "head": { "rotation": 0, "position": { "x": 0, "y": 0 } },
        "arm_left": { "rotation": -0.2, "position": { "x": 0, "y": 0 } },
        "arm_right": { "rotation": 0.2, "position": { "x": 0, "y": 0 } },
        "leg_left": { "rotation": 0.1, "position": { "x": 0, "y": 0 } },
        "leg_right": { "rotation": -0.1, "position": { "x": 0, "y": 0 } }
      }
    }
  ]
}
```

---

## 六、命名规范

### 6.1 文件命名

| 类型 | 规范 | 示例 |
|------|------|------|
| 模型类 | PascalCase + `.cs` | `Bone.cs`, `Animation.cs` |
| 核心算法类 | PascalCase + `.cs` | `AnimationPlayer.cs`, `Matrix3x3.cs` |
| 服务类 | PascalCase + `Loader.cs`/`Manager.cs` | `CharacterLoader.cs`, `ResourceManager.cs` |
| 控件类 | PascalCase + `.xaml/.xaml.cs` | `CharacterCanvas.xaml` |
| 角色配置文件 | snake_case + `.json` | `skeleton.json`, `attachments.json` |
| 动画文件 | snake_case + `.json` | `idle.json`, `walk.json` |
| 图片资源 | snake_case + `.png` | `normal.png`, `happy.png` |

### 6.2 类命名

| 类型 | 规范 | 示例 |
|------|------|------|
| 模型 | PascalCase | `Bone`, `Animation`, `Attachment` |
| 核心算法 | PascalCase | `AnimationPlayer`, `SkeletonTransform` |
| 服务 | PascalCase + `Loader`/`Manager` | `CharacterLoader`, `ResourceManager` |
| 渲染器 | PascalCase + `Renderer` | `SkeletonRenderer`, `WpfRenderer` |
| 工具类 | PascalCase | `Matrix3x3`, `Interpolator`, `Bezier` |

### 6.3 资源命名

| 类型 | 规范 | 示例 |
|------|------|------|
| 角色目录 | PascalCase | `KfuPet/` |
| 图片子目录 | snake_case | `head/`, `arm_left/`, `accessories/` |
| 图片文件名 | snake_case | `normal.png`, `outfit_summer.png` |
| 表情资源 | emotion_action | `happy.png`, `blink.png`, `sleep.png` |

### 6.4 骨骼命名

| 部位 | 规范 | 示例 |
|------|------|------|
| 根节点 | `root` | root |
| 身体 | `body` | body |
| 头部 | `head` | head |
| 四肢 | `{limb}_{side}` | arm_left, arm_right, leg_left, leg_right |
| 手指/脚趾 | `{limb}_{side}_{digit}` | arm_left_index, leg_right_big_toe |

---

## 七、注意事项

### 7.1 性能优化

1. **骨骼数量控制**：建议单角色骨骼数量不超过 20 个
2. **资源缓存**：使用 `ResourceManager` 缓存已加载的图片和配置
3. **变换缓存**：缓存计算好的世界变换矩阵
4. **对象池复用**：避免频繁创建和销毁 UI 元素
5. **渲染优化**：使用 Canvas 而非 Grid，减少布局计算开销

### 7.2 扩展性设计

1. **数据驱动**：所有配置通过 JSON 文件定义，支持动态加载
2. **插件化架构**：Core 层设计为可替换的接口实现
3. **角色包机制**：支持多个角色包，运行时动态切换
4. **渲染抽象**：`Renderer` 基类支持多种渲染后端（WPF、SkiaSharp）

### 7.3 兼容性

1. **DPI 感知**：参考现有 `GetDpiScale` 方法处理高 DPI 屏幕
2. **透明窗口**：保持与现有主窗口一致的透明无边框风格
3. **资源加载**：使用 `pack://application:,,,/` URI 方案加载资源

### 7.4 多附件设计的优势

| 功能 | 实现方式 |
|------|----------|
| **换衣服** | 切换 body 附件的资源 |
| **换发型** | 切换 hair 附件的资源 |
| **戴眼镜** | 设置 glasses 附件可见并选择资源 |
| **戴帽子** | 添加 hat 附件并设置可见 |
| **AI 表情切换** | 根据情绪状态切换 face/eyes 附件的资源 |
| **眨眼动画** | 短暂切换 eyes 附件到 blink 资源后恢复 |

### 7.5 未来扩展

```
Core/
├── AI/                    # AI 对话、动画生成
│   ├── AIClient.cs
│   ├── AIAnimationGenerator.cs
│   └── AIMemory.cs
├── Memory/                # 记忆系统
│   ├── ShortTermMemory.cs
│   ├── LongTermMemory.cs
│   └── MemoryManager.cs
├── Emotion/               # 情绪系统
│   ├── EmotionState.cs
│   ├── EmotionController.cs
│   └── EmotionTrigger.cs
├── Physics/               # 物理系统
│   ├── PhysicsEngine.cs
│   ├── PhysicsBone.cs
│   └── CollisionDetector.cs
└── Plugin/                # 插件系统（后期）
    ├── PluginManager.cs
    └── IPlugin.cs
```

> KfuPet项目组