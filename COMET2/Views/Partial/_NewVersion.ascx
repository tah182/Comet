<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div class="top-left">
    <h2><b>A New Version has been released</b></h2>
</div>

<div class="menu-change">
    
</div>

<div class="body-change">
    <b>New With Version 3.1.2:</b>
    <ul>
        <li>Make them stick!
            <ul>
                <li>Now you can make the details on the right of the planning page stick by clicking on them.</li>
            </ul>
        </li>
        <li>Limitations removed!
            <ul>
                <li>Bulk CLLI no longer limits you to just 150. There is no longer any limitations to the amount you can receive back.</li>
            </ul>
        </li>
    </ul>
    <br /><br />
    <b>HISTORY: New With Version 3.0.1:</b>
    <ul>
        <li>Added ability to right-click on the map and search around a location.</li>
        <li>Added ability to remove search circle by clicking on drop pin.</li>        
        <li>Added ability to remove drop pin by clicking on pin and selecting "Remove Pin".</li>
        <li>Added Legacy Company information in EF Tab.</li>
        <li>Bugs / Issues
            <ol>
                <li>Selecting Number of LSEF/HSEF/NNI did not bring up EF Tab.</li>
            </ol>
        </li>
    </ul>

</div>

<div class="footer-change">

</div>

<div class="acknowledge">
    <input type="button" value="OK" onclick="closeNav(false)" />
    <input type="button" value="Don't Show Again" onclick="closeNav(true)" />
</div>