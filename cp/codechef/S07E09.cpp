#include<bits/stdc++.h>
using namespace std;

#define int         long long
#define ull         unsigned long long
#define ll             long long
#define pb             push_back
#define all(a)      (a).begin(),(a).end()
#define sz(x)       (ll)x.size()
#define endl        '\n'
#define gcd(a,b)    __gcd((a),(b))
#define lcm(a,b)    ((a)*(b)) / gcd((a),(b))
#define ios            ios_base::sync_with_stdio(false);cin.tie(0);cout.tie(0);
#define mp             make_pair
#define lb             lower_bound
#define ub            upper_bound
#define F           first
#define S           second
#define rep(i, begin, end) for (__typeof(end) i = (begin) - ((begin) > (end)); i != (end) - ((begin) > (end)); i += 1 - 2 * ((begin) > (end)))
#define hell         (ull)1e9

signed main(void)
{
    ios
    freopen("input.txt", "r", stdin);freopen("output.txt", "w", stdout);

    int TESTS=1, N=1;
    cin>>TESTS;

    while(TESTS--){
        cin>>N;
        
        vector<int> arr(N);
        rep(i, 0, N) cin>>arr[i];
        sort(all(arr));

        int mi = arr[0];
        int ans = N*arr[0];
        rep(i, 0, N) arr[i] -= mi;

        vector<int> zeros;
        rep(i, 0, N) if(arr[i]==0) zeros.pb(i);
        
        int mx = 0;
        rep(i, 0, sz(zeros)-1) mx = max(mx, zeros[i+1]-zeros[i]-1);
        mx = max(mx, zeros[0] + (N-1) - zeros[sz(zeros)-1]);

        cout << ans+mx << "\n";
    }
    return 0;
}