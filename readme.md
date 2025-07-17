# workflow-engine

[![C#](https://img.shields.io/badge/language-CSharp-blue.svg)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/framework-.NET_8.0-blueviolet.svg)](https://dotnet.microsoft.com/)
[![GitHub](https://img.shields.io/badge/hosted_on-GitHub-black?logo=github)](https://github.com/divyanshjain/workflow-engine)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg)](LICENSE)

A super simple workflow engine that lets you create digital approval processes! Think of it like building LEGO blocks for business workflows - you define the steps, connect them with actions, and watch things move through your process automatically.

Perfect for things like document approvals, vacation requests, or any multi-step process where something needs to go from "start" to "finish" with people clicking buttons along the way.

---

## 🎯 What This Does

- **Create Templates**: Design workflow blueprints (like "Document Approval Process")
- **Start Workflows**: Launch real workflows from your templates (like "John's vacation request")
- **Move Things Forward**: Click buttons to push workflows to their next stage
- **Stay Safe**: Built-in rules prevent invalid moves (can't approve something that's still in draft!)
- **Remember Everything**: Complete history of who did what and when
- **Zero Setup**: No database needed - just run and go!

---

## 🏃‍♂️ Get Started in 30 Seconds
### ✅ What You Need

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download) (the free Microsoft thing)
- Any tool to test APIs (Postman, or just use the built-in Swagger docs!)

### ▶️ Fire It Up

```bash
# Grab the code
git clone https://github.com/divyanshjain/workflow-engine.git
cd workflow-engine

# Run it (that's literally it!)
dotnet run
```

🎉 **BOOM!** Your workflow engine is now running at `http://localhost:5xxx`

Visit `/swagger` in your browser for a nice interactive API playground!

---

## 🗺️ How to Use This Thing

| What You Want to Do | API Call | What Happens |
|---------------------|----------|--------------|
| Create a workflow template | `POST /api/workflow/definitions` | Make a new blueprint others can use |
| See all your templates | `GET /api/workflow/definitions` | List everything you've built |
| Get one specific template | `GET /api/workflow/definitions/{id}` | See the details of one blueprint |
| Start a real workflow | `POST /api/workflow/instances` | Launch something following a template |
| Move workflow forward | `POST /api/workflow/instances/{id}/actions/{action}` | Click a button to advance the process |
| Check workflow status | `GET /api/workflow/instances/{id}` | See where something is right now |

---

## 🎮 Try It Out - Document Approval Example

### Step 1: Create Your First Workflow Template

```json
POST /api/workflow/definitions
{
  "id": "document-approval",
  "states": [
    {"id": "draft", "isInitial": true, "isFinal": false, "enabled": true},
    {"id": "review", "isInitial": false, "isFinal": false, "enabled": true},
    {"id": "approved", "isInitial": false, "isFinal": true, "enabled": true},
    {"id": "rejected", "isInitial": false, "isFinal": true, "enabled": true}
  ],
  "actions": [
    {"id": "submit", "enabled": true, "fromStates": ["draft"], "toState": "review"},
    {"id": "approve", "enabled": true, "fromStates": ["review"], "toState": "approved"},
    {"id": "reject", "enabled": true, "fromStates": ["review"], "toState": "rejected"}
  ]
}
```

### Step 2: Start Using Your Template

```json
POST /api/workflow/instances
{
  "definitionId": "document-approval"
}
```

This gives you back an instance ID - that's your workflow's unique name!

### Step 3: Move It Through the Process

```bash
# Submit the document for review
POST /api/workflow/instances/{your-instance-id}/actions/submit

# Manager approves it
POST /api/workflow/instances/{your-instance-id}/actions/approve
```

### Step 4: Check What Happened

```bash
GET /api/workflow/instances/{your-instance-id}
```

You'll see the current state and complete history of moves!

---

## 🧠 How I Built This (For Fellow Developers)

### Architecture That Makes Sense
- **Controllers**: Handle HTTP stuff (parsing JSON, returning status codes)
- **Services**: All the business logic and validation rules
- **Storage**: Simple in-memory data storage (swap for database later!)
- **Models**: Clean data structures that represent real concepts

### Why These Choices?
- **Layered Architecture**: Each piece has one job and does it well
- **Interface-Based**: Easy to swap parts (like changing storage)
- **Validation-Heavy**: Lots of checks to prevent weird states
- **Error-Friendly**: Clear error messages when things go wrong

---

## 👨‍💻 Built With ❤️ By
**Divyansh Jain**  
IIT Kharagpur '27  
📧 div0211jain@gmail.com