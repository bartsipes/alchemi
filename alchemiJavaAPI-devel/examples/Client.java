
/**
 * 
 */

/**
 * @author Krishna
 *
 */
public class Client {

	/**
	 * 
	 */
	public Client() {
	}

	/**
	 * @param args
	 * @throws Exception 
	 */
	public static void main(String[] args) throws Exception {
		System.out.println("Started executing ... ");
		
		System.out.println("Hostname : " + java.net.InetAddress.getLocalHost().getHostName());
		System.out.println("Start Time millis : " + System.currentTimeMillis());
		Thread.sleep(2000);
		System.out.println("End Time millis : " + System.currentTimeMillis());
		
		System.out.println("Done. ");
	}

}
