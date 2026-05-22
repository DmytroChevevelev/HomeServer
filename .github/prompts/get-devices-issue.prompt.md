---
name: get-devices-issue
description: Investigate frontend cannot get devices and hand off findings to speckit.specify
---

<!-- Tip: Use /create-prompt in chat to generate content with agent assistance -->

The device list page in the frontend is not displaying any devices, even though there are registered devices in the database. The swagger UI returns the correct list of devices when calling the /api/devices endpoint, but the frontend shows an empty list. Investigate the issue and find the root cause. Check the frontend code for the device list page, especially the service that calls the backend API and the component that displays the devices. Look for any errors in the console or network requests that might indicate what is going wrong.