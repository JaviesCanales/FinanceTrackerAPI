## Bug Fixes

### Duplicate ID Bug
**Problem:** When a transaction was deleted and a new one was added, 
the new transaction received the same ID as an existing one because 
the ID was based on list count.

**Example:** 
- Add transaction → id 1
- Add transaction → id 2  
- Delete id 1 → list count is now 1
- Add transaction → id 2 again (duplicate)

**Fix:** Replaced list count with a static `nextId` counter that 
increments permanently and never resets, preventing duplicate IDs 
regardless of deletions.