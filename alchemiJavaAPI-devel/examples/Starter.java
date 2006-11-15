import org.gridbus.alchemi.client.EmbeddedFileDependency;
import org.gridbus.alchemi.client.FileDependency;
import org.gridbus.alchemi.client.FileDependencyCollection;
import org.gridbus.alchemi.client.GApplication;
import org.gridbus.alchemi.client.GJob;
import org.gridbus.alchemi.client.IJobListener;
import org.gridbus.alchemi.client.SecurityCredentials;

/**
 * 
 */

/**
 * @author Krishna
 *
 */
public class Starter implements IJobListener {

	private static final String workingDir = "./workingDIR";
	/**
	 * @param args
	 * @throws Exception 
	 */
	public static void main(String[] args) throws Exception {
		
		//create credentials
		SecurityCredentials sec = new SecurityCredentials("user","user");
		
		//create grid app
		GApplication ga = new GApplication("http://192.168.0.102/XPManager/CrossplatformManager.asmx", sec);
		ga.setLocalWorkingDirectory(workingDir);
		
		for (int i = 0; i < 12; i++){
			//create a job
			GJob job = new GJob();
			job.setRunCommand("java -cp . Client");
			
			//set application dependencies
			job.getInputfiles().add(new EmbeddedFileDependency("bin\\Client.class"));
			
			//add job
			ga.addJob(job);
		}
		
		//subscribe to events
		ga.addListener(new Starter());
		
		//launch the app
		ga.start();
		
		System.out.println("Started GApp with " + ga.getJobs().size() + " jobs ... ");
	}

	/**
	 * 
	 */
	public void ApplicationFinished() {
		System.out.println("Application finished");
	}

//	private void unpackFiles(GJob job){
//		FileDependencyCollection outputs = job.getOutputfiles();
//		for (int i = 0; i < outputs.size(); i++){
//			FileDependency dep = outputs.get(i);
//			dep.UnPack(workingDir + "/" + "job_" + job.getJobID() + "/" + dep.getFilename());
//		}
//	}
	
	/**
	 * 
	 */
	public void JobFailed(GJob job, Exception ex) {
		System.out.println("Job " + job.getJobID() + " failed : " + ex.toString());
		//unpackFiles(job);
	}

	/**
	 * 
	 */
	public void JobFinished(GJob job) {
		System.out.println("Job " + job.getJobID() + " done ! ");
		//unpackFiles(job);
	}
}
