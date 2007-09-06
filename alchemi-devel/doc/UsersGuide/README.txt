How to Create the User's Guide Documentation
9/6/07 Matt Valerio

Requirements
============
Install Microsoft HTML Help Workshop

It can be downloaded from here:
http://www.microsoft.com/downloads/details.aspx?FamilyID=00535334-c8a6-452f-9aa0-d597d16580cc&displaylang=en
Get htmlhelp.exe (it's a self-extracting zip)


Background
==========
This project uses a customized MsBuild script to transform each XML documentation file into an HTML file according to a template.

The template file is "template.html".  The contents of each XML file is inserted where the "$content$" placeholder is to produce a temporary HTML file.

HTML Help Workshop is used to edit the "UsersGuide.hhp" project file and the "toc.hhc" table-of-contents files.


Building
========
By right-clicking on the UsersGuide project in Visual Studio, select "Build".  This will create HTML files corresponding to each XML file.

Then, in HTML Help Workshop, on the "Projects" tab, click the bottommost button to build the project.  This creates UsersGuid.chm.

Alternately, the following command may be run:
C:\Program Files\HTML Help Workshop\hhc.exe <path-to-alchemi\alchemi-devel\doc\UsersGuide\UsersGuide.hhp


Adding a topic
==============
- Create the necessary folder under the "html" subdirectory, if required
- Create an empty file in that folder, like "newtopic.xml"
  Note: All xml files must be 1 and only 1 folder level deep, since the template
        references "..\alchemi.css" and "..\alchemi.jpg"
- Edit the UsersGuide.proj file and add the path to the new topic XML file in the "XmlFiles" ItemGroup
  Note: In Visual Studio, follow the following steps:
  - Right-click on the project
  - Click "Unload project"
  - Right-click on the project
  - Click "Edit UsersGuid.proj"
  ...
  - Right-click on the project
  - Click "Reload project"
  - Click "Yes" to save everything
- When you reload the project, the XML file will show up in the Solution Explorer and you can edit it.


Adding Images
=============
Use the following code to add an image to be consistent with the rest of the document:
<div class="figure">
  <img alt="Alt Text" src="images\image.jpg" />
</div>
<div class="caption">
  The Caption
</div>
